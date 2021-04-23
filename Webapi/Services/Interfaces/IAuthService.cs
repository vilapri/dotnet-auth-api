using Webapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Register(RegisterModel registerModel);
        Task<LoginResponse> Login(LoginModel loginModel);
        Task ChangeAccount(Guid userId, AccountChangeRequest accountChangeRequest);
        Task ChangePassword(Guid userId, PasswordChangeRequest passwordChangeRequest);
    }
}