  
using Webapi.Models;
using Webapi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Webapi.Models.Enums;

namespace Webapi.Controllers
{
    [Authorize]
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

        [HttpPut("change-account")]
        public async Task<IActionResult> ChangeAccount(AccountChangeRequest accountChangeRequest)
        {
            Guid userId = _claimsService.GetGuidClaim(HttpContext.User, EClaimTypes.UserId).Value;

            await _authService.ChangeAccount(userId, accountChangeRequest);

            return Ok();
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(PasswordChangeRequest passwordChangeRequest)
        {
            Guid userId = _claimsService.GetGuidClaim(HttpContext.User, EClaimTypes.UserId).Value;

            await _authService.ChangePassword(userId, passwordChangeRequest);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (String.IsNullOrEmpty(registerModel.UserName)
                || String.IsNullOrEmpty(registerModel.Email)
                || String.IsNullOrEmpty(registerModel.Password)
                || String.IsNullOrEmpty(registerModel.ConfirmPassword))
            {
                return BadRequest("Missing fields");
            }

            if (registerModel.Password != registerModel.ConfirmPassword)
            {
                return BadRequest("Password missmatch");
            }

            try
            {
                bool registerSuccess = await _authService.Register(registerModel);

                if (!registerSuccess)
                {
                    return BadRequest("Unexpected Error");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [AllowAnonymous]
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