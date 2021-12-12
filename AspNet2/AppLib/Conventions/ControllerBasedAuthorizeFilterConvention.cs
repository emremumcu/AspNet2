
using AspNet2.AppLib.StartupExt;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNet2.AppLib.Conventions
{
    /*
        IApplicationModelConvention
        IControllerModelConvention
        IActionModelConvention
        IParameterModelConvention
     */

    public class ControllerBasedAuthorizeFilterConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerName.StartsWith("Admin"))
            {
                controller.Filters.Add(new AuthorizeFilter(AuthorizationPolicyLibrary.adminPolicy));
            }
            else if (controller.ControllerName.StartsWith("Account"))
            {
                controller.Filters.Add(new AllowAnonymousFilter());
            }
            else
            {
                controller.Filters.Add(new AuthorizeFilter(AuthorizationPolicyLibrary.defaultPolicy));
            }
        }
    }

    //public class MyModelBinderConvention : IActionModelConvention
    //{
    //    public void Apply(ActionModel action)
    //    {
            
            
         
    //    }
    //}
}
