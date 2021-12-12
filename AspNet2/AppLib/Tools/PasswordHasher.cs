using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNet2.AppLib.Tools
{
    public class PasswordHasher
    {
        
         // The Microsoft. AspNetCore. Identity. PasswordHasher is installed with the target
         // framework netcoreapp3.1. This PasswordHasher defaults to IdentityV3. IdentityV3 is
         // encrypted PBKDF2 with HMAC-SHA256, 128-bit salt, 256-bit subkey, 10000 iterations.
         // IdentityV3 is formated: { 0x01, prf(UInt32), iter count(UInt32), salt length(UInt32),
         // salt, subkey }.
         
    }
}
