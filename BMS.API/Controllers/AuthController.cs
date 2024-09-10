using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.User;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services;
using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

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
                async () => await _authService.RegisterAsync(request).ConfigureAwait(false)
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
            return await ExecuteServiceLogic(
                async () => await _authService.ConfirmEmail(userId, token).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // Remove the JWT token cookie
            HttpContext.Response.Cookies.Delete("token");
            return Ok();
        }

  
    }
}
