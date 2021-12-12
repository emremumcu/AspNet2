using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNet2.AppLib.Constants
{
    public static class AppSettings
    {
        public const string SessionCookieName = "__app__session__";
     
        /// <summary>
        /// In seconds
        /// </summary>
        public const int SessionIdleTimeout = 1200;
        
        public const string AuthenticationCookieName = "__app__auth__";

        public const string AuthenticationLoginPath = "/Account/Login";

        public const string AuthenticationLogoutPath = "/Account/Logout";

        public const string AuthenticationAccessDeniedPath = "/Account/AccessDenied";

        /// <summary>
        /// In seconds
        /// </summary>
        public const int AuthenticationExpireTimeSpan = 1200;

        public const string AuthenticationClaimsUser = "__app__";

        public const string AuthenticationReturnUrlParameter = "ReturnUrl";
    }
}
