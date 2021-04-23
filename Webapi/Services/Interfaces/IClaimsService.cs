  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Webapi.Services.Interfaces
{
    public interface IClaimService
    {
        Guid? GetGuidClaim(ClaimsPrincipal claimsPrincipal, string claimType);
    }
}