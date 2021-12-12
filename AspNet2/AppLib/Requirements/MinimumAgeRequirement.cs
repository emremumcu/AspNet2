using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNet2.AppLib.Requirements
{
    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public int Age { get; private set; }

        public MinimumAgeRequirement(int minimumAge)
        {
            Age = minimumAge;
        }
    }
}
