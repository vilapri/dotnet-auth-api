  
using Webapi.Models;
using Webapi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapi.Models.Enums;

namespace Webapi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly IClaimService _claimsService;

        public AuthController(
            IAuthService authService,
            IJwtService jwtService,
            IClaimService claimsService)
        {
            _authService = authService;
            _jwtService = jwtService;
            _claimsService = claimsService;
        }

        [HttpPost("account")]
        public async Task<IActionResult> ChangeAccount(AccountChangeRequest accountChangeRequest)
        {
            Guid userId = _claimsService.GetGuidClaim(HttpContext.User, EClaimTypes.UserId).Value;

            await _authService.ChangeAccount(userId, accountChangeRequest);

            return Ok();
        }

        [HttpPost("password")]
        public async Task<IActionResult> ChangePassword(PasswordChangeRequest passwordChangeRequest)
        {
            Guid userId = _claimsService.GetGuidClaim(HttpContext.User, EClaimTypes.UserId).Value;

            await _authService.ChangePassword(userId, passwordChangeRequest);

            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (String.IsNullOrEmpty(registerModel.UserName)
                || String.IsNullOrEmpty(registerModel.Email)
                || String.IsNullOrEmpty(registerModel.Password)
                || String.IsNullOrEmpty(registerModel.ConfirmPassword))
            {
                throw new Exception("Missing fields");
            }

            if (registerModel.Password != registerModel.ConfirmPassword)
            {
                throw new Exception("Password missmatch");
            }

            bool registerSuccess = await _authService.Register(registerModel);

            if (!registerSuccess)
            {
                throw new Exception("Unexpected error");
            }

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            LoginResponse loginResponse = await _authService.Login(loginModel);

            if (loginResponse.Success)
            {
                JwtModel jwtModel = new JwtModel
                {
                    ApplicationUserId = loginResponse.ApplicationUserId,
                    UserRoles = loginResponse.UserRoles
                };

                string token = _jwtService.GenerateSecurityToken(jwtModel);

                LoginResult loginResult = new LoginResult
                {
                    Token = token
                };

                return Ok(loginResult);
            }
            else
            {
                return BadRequest("Incorrect login");
            }
        }
    }
}