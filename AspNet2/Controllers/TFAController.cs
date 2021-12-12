using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AspNet2.AppLib.Tools;
using AspNet2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtpNet;

namespace AspNet2.Controllers
{
    public class TFAManager
    {
        public const string AppUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        /// <summary>
        /// Generates a random 160-bit Base32-Encoded security key 
        /// </summary>
        /// <returns></returns>
        public static string GetAuthenticatorKey()
        {
            using (RandomNumberGenerator generator = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[20];  // size of SHA1 hash          
                generator.GetBytes(bytes);
                return Base32.ToBase32(bytes);
            }
        }

        public static string FormatKey(string unformattedKey)
        {
            int currentPosition = 0;
            
            StringBuilder result = new StringBuilder();
            
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }

            if (currentPosition < unformattedKey.Length) result.Append(unformattedKey.Substring(currentPosition));

            return result.ToString().ToLowerInvariant();
        }

        public static string QrCodeUri(string caption, string name, string unformattedKey)
        {
            return string.Format(AppUriFormat, System.Web.HttpUtility.UrlEncode(caption), System.Web.HttpUtility.UrlEncode(name), unformattedKey);
        }

        public static int GetAuthenticatorCode(string unformattedKey)
        {
            long unixTimestamp = (DateTime.UtcNow.Ticks - 621355968000000000L) / 10000000L;

            long window = unixTimestamp / (long)30;

            byte[] keyBytes = Base32.FromBase32(unformattedKey);

            byte[] counter = BitConverter.GetBytes(window);

            if (BitConverter.IsLittleEndian) Array.Reverse(counter);

            HMACSHA1 hmac = new HMACSHA1(keyBytes);

            byte[] hash = hmac.ComputeHash(counter);

            int offset = hash[^1] & 0xf;

            // Convert the 4 bytes into an integer, ignoring the sign.
            var binary = ((hash[offset] & 0x7f) << 24) | (hash[offset + 1] << 16) | (hash[offset + 2] << 8) | (hash[offset + 3]);

            return binary % (int)Math.Pow(10, 6);
        }

        public static long GetCurrentCounter()
        {
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return 30 - (long)(DateTime.UtcNow - unixEpoch).TotalSeconds % 30;
        }
    }

    [AllowAnonymous]
    public class TFAController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            string randomKey = TFAManager.GetAuthenticatorKey();

            TFAViewModel vm = new TFAViewModel();

            vm.User = User.Identity as System.Security.Claims.ClaimsIdentity;
            vm.RandomKey = randomKey;
            vm.RandomKeyFormatted = TFAManager.FormatKey(randomKey);
            vm.QRCodeData = TFAManager.QrCodeUri("AspNet2", "Emre", randomKey);

            return View(model: vm);
        }

        private JsonResult CreateCode(string authenticatorKey)
        {
            try
            {
                if (string.IsNullOrEmpty(authenticatorKey)) throw new ArgumentNullException(nameof(authenticatorKey));

                AuthCodeViewModel vm = new AuthCodeViewModel();

                // Code & Remaining Time From TFAManager
                // var code = TwoFactorAuth.GetAuthenticatorCode(authenticatorKey);
                // var remainingTime = TwoFactorAuth.GetCurrentCounter();

                // Code & Remaining Time From Otp.NET
                // https://github.com/kspearrin/Otp.NET
                // var totp = new Totp(Base32Encoding.ToBytes(authenticatorKey));
                var totp = new Totp(Base32Encoding.ToBytes(authenticatorKey), timeCorrection: new TimeCorrection(DateTime.UtcNow.AddSeconds(+1)));
                var code = totp.ComputeTotp();
                var remainingTime = totp.RemainingSeconds();

                vm.AuthCode = $"{code.PadLeft(6, '0')}";
                vm.RemainingTime = $"{remainingTime, 2:00}";

                return Json(vm);
            }
            catch (Exception ex)
            {
                AuthCodeViewModel vm = new AuthCodeViewModel() { AuthCode = "0", RemainingTime = "0", Message = ex.Message };
                return Json(vm);
            }
        }

        private string OtpVerify(string authenticatorKey, string totpCode)
        {
            var totp = new Totp(Base32Encoding.ToBytes(authenticatorKey));

            long timeWindowUsed;
            //var window = new VerificationWindow(previous: 1, future: 1);
            //bool result = totp.VerifyTotp(totpCode, out timeWindowUsed, window);
            bool result = totp.VerifyTotp(totpCode, out timeWindowUsed, VerificationWindow.RfcSpecifiedNetworkDelay);

            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime timeWindowDate = epoch.AddSeconds(timeWindowUsed);

            return $"OTP: {result} {timeWindowUsed} {timeWindowDate.ToLongDateString()} {timeWindowDate.ToLongTimeString()}";
        }


        [HttpPost]
        public IActionResult Index(TFAViewModel model)
        {
            string authenticatorKey = model.RandomKey;

            return View(viewName: "Validate", model: authenticatorKey);
        }

        [HttpPost]
        public IActionResult ValidateResult(string authkey, string authcode)
        {
            string result = OtpVerify(authkey, authcode);

            return View(model: result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("get-auth-code")]
        public IActionResult GetAuthCode(string authenticatorKey)
        {
            return CreateCode(authenticatorKey);
        }
    }
}
