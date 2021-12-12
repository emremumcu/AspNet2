using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNet2.ViewModels
{
    public class TFAViewModel
    {
        public ClaimsIdentity User { get; set; }

        public String UserId 
        { 
            get { return User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value ?? throw new ArgumentException(nameof(ClaimTypes.NameIdentifier)); } 
        }

        public String UserName 
        { 
            get { return User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault()?.Value ?? throw new ArgumentException(nameof(ClaimTypes.Name)); } 
        }

        public String RandomKey { get; set; }
        
        public String RandomKeyFormatted { get; set; }
        
        public String QRCodeData { get; set; }
    }
}
