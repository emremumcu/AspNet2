using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet2.AppLib.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace AspNet2.Controllers
{
    public class CustomController : Controller
    {
        [AppAuthorizeAttribute(Privileges = "")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
