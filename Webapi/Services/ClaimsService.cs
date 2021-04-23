using Webapi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Webapi.Services
{
    public class ClaimService : IClaimService
    {
        public Guid? GetGuidClaim(ClaimsPrincipal claimsPrincipal, string claimType)
        {
            string claimValue = claimsPrincipal.Claims
                .FirstOrDefault(c => c.Type == claimType)?
                .Value;

            if (Guid.TryParse(claimValue, out Guid result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}