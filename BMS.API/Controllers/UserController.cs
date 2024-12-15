using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Models.Requests.Users;
using BMS.BLL.Services;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
            _baseService = (BaseService)userService;
        }

        [HttpDelete]
        [Authorize(Roles = UserRoleConstants.ADMIN + "," + UserRoleConstants.STAFF)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            return await ExecuteServiceLogic(
                async () => await _userService.DeleteUser(id).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
        [HttpGet("GetListUser")]
        [Authorize(Roles = UserRoleConstants.ADMIN + "," + UserRoleConstants.STAFF)]
        public async Task<IActionResult> GetListUser([FromQuery]SearchStaffRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _userService.GetListUser(request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetUserById{id}")]
        [Authorize(Roles = UserRoleConstants.ADMIN + "," + UserRoleConstants.STAFF)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            return await ExecuteServiceLogic(
                async () => await _userService.GetUserByID(id).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetAllOrderAndFeedbackOfUser{id}")]
        [Authorize(Roles = UserRoleConstants.ADMIN + "," + UserRoleConstants.STAFF)]
        public async Task<IActionResult> GetAllOrderAndFeedbackOfUser(Guid id)
        {
            return await ExecuteServiceLogic(
                async () => await _userService.GetAllOrderAndFeedbackOfUser(id).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetUserByEmail{email}")]
        [Authorize(Roles = UserRoleConstants.ADMIN + "," + UserRoleConstants.STAFF)]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            return await ExecuteServiceLogic(
                async () => await _userService.GetUserByEmail(email).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetTotalUser")]
        [Authorize(Roles = UserRoleConstants.ADMIN + "," + UserRoleConstants.STAFF)]
        public async Task<IActionResult> GetTotalUser()
        {
            return await ExecuteServiceLogic(
                async () => await _userService.GetTotalUser().ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
        [HttpGet("CountNewUser")]
        [Authorize(Roles = UserRoleConstants.ADMIN + "," + UserRoleConstants.STAFF)]
        public async Task<IActionResult> CountNewUser([FromQuery] TotalUserRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _userService.CountNewUser(request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
    }
}
