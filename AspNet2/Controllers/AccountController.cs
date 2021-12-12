using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet2.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AspNet2.AppLib.StartupExt;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using AspNet2.AppLib.Tools;
using System.IO;
using Microsoft.AspNetCore.Http;
using AspNet2.AppLib.Constants;

namespace AspNet2.Controllers
{
    // TODO: ControllerBasedAuthorizeFilterConvention did NOT work for [AllowAnonymous]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private async Task<IActionResult> LoginUser(string username, string password, bool remember)
        {
            bool isUserCredentialsValid = username == "123" && password == "123";

            if(isUserCredentialsValid)
            {
                List<Claim> userClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, "UserNameIdentifier"),
                    new Claim(ClaimTypes.Name, "UserName"),
                    new Claim(ClaimTypes.Role, "UserRole")
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Manuel Login:
                // HttpContext.RequestServices.GetRequiredService<IAuthenticationService>().SignInAsync(HttpContext, "cookies", claimsPrincipal, pr);
                // HttpContext.User = claimsPrincipal;

                HttpContext.Session.SetKey<bool>("login", true);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrincipal,
                    new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(10),
                        IsPersistent = (true),
                        IssuedUtc = DateTime.UtcNow
                    }
                );

                // System.Security.Claims.ClaimsIdentity ci = ((System.Security.Claims.ClaimsIdentity)User.Identity);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                System.Threading.Thread.Sleep(5000);
                throw new Exception("Kullanıcı bilgileri hatalı.");
            }
        }

        [HttpGet]
        public IActionResult Login() => View(model: new LoginViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string submit)
        {
            // model içindeki CaptchaCode temizlense bile, 
            // ModelState değeri de temizlenmeden CaptchaCode textbox değeri dolu geliyor
            void ClearCaptchaText()
            {
                model.Captcha.CaptchaCode = string.Empty;
                ModelState.SetModelValue("Captcha.CaptchaCode", new ValueProviderResult(string.Empty));
            }

            try
            {
                if (ModelState.IsValid)
                {
                    if (!Captcha.ValidateCaptchaCode(model.Captcha.CaptchaCode, HttpContext))
                    {
                        System.Threading.Thread.Sleep(5000);
                        ModelState.AddModelError("Captcha", "Güvenlik kodu yanlış.");
                        ClearCaptchaText();
                        return View(model);
                    }
                    else
                    {
                        return await LoginUser(model.Username, model.Password, model.RememberMe);
                    }
                }
                else
                {
                    System.Threading.Thread.Sleep(5000);
                    ModelState.AddModelError("ERR", $"Formda hatalar var. Lütfen hataları düzeltip, işleminizi yeniden deneyiniz.");
                    ClearCaptchaText();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ERR", $"Hata: {ex.Message}");
                ClearCaptchaText();
                return View(model);
            }
        }

        public IActionResult AccessDenied()
        {
            return Content("Access Denied!");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("login");         
            
            HttpContext.Session.Clear();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var cookie = this.Request.Cookies[AppSettings.AuthenticationCookieName];
            
            if (cookie != null)
            {
                var options = new CookieOptions { Expires = DateTime.Now.AddDays(-1) };
                this.Response.Cookies.Append(AppSettings.AuthenticationCookieName, cookie, options);
            }
            
            return RedirectToAction("Login");
        }

        [Route("get-captcha-image")]
        public IActionResult GetCaptchaImage()
        {
            int width = 100;
            int height = 36;
            var captchaCode = Captcha.GenerateCaptchaCode();
            var result = Captcha.GenerateCaptchaImage(width, height, captchaCode);
            HttpContext.Session.SetString("CaptchaCode", result.CaptchaCode);
            Stream s = new MemoryStream(result.CaptchaByteData);
            return new FileStreamResult(s, "image/png");
        }
    }
}

