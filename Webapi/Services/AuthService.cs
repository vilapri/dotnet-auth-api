using Webapi.DataAccess.Entities;
using Webapi.Models;
using Webapi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapi.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ChangeAccount(Guid userId, AccountChangeRequest accountChangeRequest)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId.ToString());

            user.UserName = accountChangeRequest.Username;
            user.Email = accountChangeRequest.Email;

            await _userManager.UpdateAsync(user);
            await _userManager.UpdateNormalizedEmailAsync(user);
            await _userManager.UpdateNormalizedUserNameAsync(user);
        }

        public async Task ChangePassword(Guid userId, PasswordChangeRequest passwordChangeRequest)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId.ToString());

            if (passwordChangeRequest.NewPassword.Equals(passwordChangeRequest.NewPasswordRepeat))
            {
                await _userManager.ChangePasswordAsync(user, passwordChangeRequest.OldPassword, passwordChangeRequest.NewPassword);
            }
        }

        public async Task<LoginResponse> Login(LoginModel loginModel)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(loginModel.Email);

            bool loginSuccess = await _userManager.CheckPasswordAsync(user, loginModel.Password);

            LoginResponse loginResponse;

            if (loginSuccess)
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);

                loginResponse = new LoginResponse
                {
                    Success = loginSuccess,
                    ApplicationUserId = user.Id,
                    UserRoles = roles.ToList()
                };

            }
            else
            {
                loginResponse = new LoginResponse
                {
                    Success = false,
                    ApplicationUserId = Guid.Empty
                };
            }

            return loginResponse;
        }

        public async Task<bool> Register(RegisterModel registerModel)
        {
            ApplicationUser newUser = new ApplicationUser
            {
                Email = registerModel.Email,
                UserName = registerModel.UserName
            };

            IdentityResult createResult = await _userManager.CreateAsync(newUser);

            if (!createResult.Succeeded)
            {
                if (createResult.Errors.Any())
                {
                    throw new Exception(String.Join(";", createResult.Errors.Select(e => e.Code).ToList()));
                }
                return false;
            }

            IdentityResult passwordResult = await _userManager.AddPasswordAsync(newUser, registerModel.Password);

            if (!passwordResult.Succeeded)
            {
                await _userManager.DeleteAsync(newUser);

                if (passwordResult.Errors.Any())
                {
                    throw new Exception(String.Join(";", passwordResult.Errors.Select(e => e.Code).ToList()));
                }
                
                return false;
            }

            return true;
        }
    }
}