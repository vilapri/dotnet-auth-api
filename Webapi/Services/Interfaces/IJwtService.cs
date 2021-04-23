using Webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapi.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateSecurityToken(JwtModel jwtModel);
    }
}