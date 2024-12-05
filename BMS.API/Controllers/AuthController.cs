using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.User;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services;
using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using BMS.Core.Domains.Entities;
using Newtonsoft.Json.Linq;
using BMS.BLL.Models.Requests.Users;

namespace BMS.API.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
            _baseService = (BaseService)_authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUser request, int role)
        {
            return await ExecuteServiceLogic(

                async () => await _authService.RegisterAsync(request, role).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUser request)
        {
            return await ExecuteServiceLogic(
                async () => await _authService.LoginAsync(request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _authService.ConfirmEmail(userId, token).ConfigureAwait(false);
            if (result.IsSuccess)
            {
                return Content("<html><body><h1>Email is confirmed!</h1><p>The Email Is Confirmed. Thank you for confirming your email.</p></body></html>", "text/html");
            }
            else
            {
                return Content("<html><body><h1>Invalid or Expired Token</h1><p>Please request a new email confirmation.</p></body></html>", "text/html");
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // Remove the JWT token cookie
            HttpContext.Response.Cookies.Delete("token");
            return Ok();
        }

        [HttpPost("SendOTP")]
        public async Task<IActionResult> SendOTP([FromForm]string email)
        {
            return await ExecuteServiceLogic(
            async () => await _authService.SendOTP(email).ConfigureAwait(false)
           ).ConfigureAwait(false);
        }

        [HttpPost("CheckOTP")]
        public async Task<IActionResult> CheckOTP([FromForm] CheckOTPRequest request)
        {
            return await ExecuteServiceLogic(
            async () => await _authService.CheckOTP(request.Email, request.OTP).ConfigureAwait(false)
           ).ConfigureAwait(false);
        }
    }
}
