using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.Feedbacks;
using BMS.BLL.Models.Requests.Users;
using BMS.BLL.Services;
using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) 
        {
            _userService = userService;
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromForm] UserLoginRequest request)
        {
            return await ExecuteServiceLogic(
                               async () => await _userService.Login(request).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register([FromForm] UserRegisterRequest request)
        {
            return await ExecuteServiceLogic(
                               async () => await _userService.RegisterUser(request).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
    }
}
