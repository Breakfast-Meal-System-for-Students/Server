using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.Users;
using BMS.BLL.Models;
using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services;

namespace BMS.API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accountService;
        private readonly IUserClaimsService _userClaimsService;
        private UserClaims _userClaims;
        public AccountController(IAccountService accountService, IUserClaimsService userClaimsService)
        {
            _accountService = accountService;
            _userClaimsService = userClaimsService;
            _userClaims = _userClaimsService.GetUserClaims();
            _baseService = (BaseService)_accountService;
        }

        [HttpGet("my-profile")]
        [Authorize]
        public async Task<IActionResult> GetMyProfile()
        {
            return await ExecuteServiceLogic(
            async () => await _accountService.GetDetails(_userClaims.UserId).ConfigureAwait(false)
           ).ConfigureAwait(false);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserDetails(Guid id)
        {
            return await ExecuteServiceLogic(
            async () => await _accountService.GetDetails(id).ConfigureAwait(false)
           ).ConfigureAwait(false);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserRequest request)
        {
            return await ExecuteServiceLogic(
            async () => await _accountService.UpdateDetails(request, _userClaims.UserId).ConfigureAwait(false)
           ).ConfigureAwait(false);
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(UpdatePasswordRequest request)
        {
            return await ExecuteServiceLogic(
            async () => await _accountService.UpdatePassword(request, _userClaims.UserId).ConfigureAwait(false)
           ).ConfigureAwait(false);
        }

        [HttpPut("update-avatar")]
        [Authorize]
        public async Task<IActionResult> UpdateAvatar( IFormFile request)
        {
            return await ExecuteServiceLogic(
            async () => await _accountService.UpdateAvatar(request, _userClaims.UserId).ConfigureAwait(false)
           ).ConfigureAwait(false);
        }

        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm]ResetPasswordRequest request)
        {
            return await ExecuteServiceLogic(
            async () => await _accountService.ResetPassword(request.Email, request.NewPassword).ConfigureAwait(false)
           ).ConfigureAwait(false);
        }
    }
}
