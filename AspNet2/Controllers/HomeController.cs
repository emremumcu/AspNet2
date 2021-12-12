using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNet2.Controllers
{
    // ControllerBasedAuthorizeFilterConvention
    public class HomeController : Controller
    {        
        [Authorize]
        // Without arguments, [Authorize] attribute only checks that the user is authenticated (default policy => RequireAuthenticatedUser())
        public IActionResult Index() => View();

        public IActionResult UserInfo() => View();


        // The Roles property indicates that users in any of the listed roles would be granted access. 
        // To require multiple roles, you can apply the Authorize attribute multiple times, or write your own filter.
        // Roles are case-sensitive
        [Authorize(Roles = "admin, system")] // OR condition
        public IActionResult Action2() => Content("");


        // The AuthenticationSchemes property is a comma-separated string listing the authentication middleware components that the authorization layer will trust in the current context. 
        // In other words, it states that access to the Backoffice Controller class is allowed only if the user is authenticated through the Cookies scheme and has any of the listed roles.
        [Authorize(Roles = "admin, system", AuthenticationSchemes = "Cookie")]
        public IActionResult Action3() => Content("");


        // Users who belong to both admin and system roles have access
        [Authorize(Roles = "admin")]
        [Authorize(Roles = "system")] // AND condition
        public IActionResult Action4() => Content("");

    }
}
