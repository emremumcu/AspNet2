using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNet2.AppLib.Evaluators
{
    // https://github.com/aspnet/Security/blob/master/src/Microsoft.AspNetCore.Authorization.Policy/PolicyEvaluator.cs
    public class TestPolicyEvaluator : IPolicyEvaluator
    {
        public virtual async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            //if (policy.AuthenticationSchemes != null && policy.AuthenticationSchemes.Count > 0)
            //{
            //    ClaimsPrincipal newPrincipal = null;
            //    foreach (var scheme in policy.AuthenticationSchemes)
            //    {
            //        var result = await context.AuthenticateAsync(scheme);
            //        if (result != null && result.Succeeded)
            //        {
            //            newPrincipal = Tools.SecurityHelper.MergeUserPrincipal(newPrincipal, result.Principal);
            //        }
            //    }

            //    if (newPrincipal != null)
            //    {
            //        context.User = newPrincipal;
            //        return AuthenticateResult.Success(new AuthenticationTicket(newPrincipal, string.Join(";", policy.AuthenticationSchemes)));
            //    }
            //    else
            //    {
            //        context.User = new ClaimsPrincipal(new ClaimsIdentity());
            //        return AuthenticateResult.NoResult();
            //    }
            //}

            //return (context.User?.Identity?.IsAuthenticated ?? false)
            //    ? AuthenticateResult.Success(new AuthenticationTicket(context.User, "context.User"))
            //    : AuthenticateResult.NoResult();



            string testScheme = "TestScheme";

            List<Claim> testClaims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, "TestNameIdentifier"),
                new Claim(ClaimTypes.Name, "TestName")
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(testClaims, testScheme);

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, new AuthenticationProperties(), testScheme)));

            await Task.Delay(1);

            context.User = claimsPrincipal;

            return AuthenticateResult.Success(new AuthenticationTicket(context.User, testScheme));
        }

        public virtual async Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy, AuthenticateResult authenticationResult, HttpContext context, object resource)
        {
            return await Task.FromResult(PolicyAuthorizationResult.Success());
        }
    }
}
