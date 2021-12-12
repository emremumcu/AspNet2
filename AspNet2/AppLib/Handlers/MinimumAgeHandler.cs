using AspNet2.AppLib.Requirements;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNet2.AppLib.Handlers
{
    public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            var user = context.User;

            if (!user.HasClaim(c => c.Type == "Age")) return Task.CompletedTask;

            var since = Convert.ToInt32(user.FindFirst("Age").Value);

            if (since >= requirement.Age) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
