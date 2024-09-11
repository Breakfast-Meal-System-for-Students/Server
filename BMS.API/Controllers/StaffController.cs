using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Models.Requests.Shop;
using BMS.BLL.Services;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
    public class StaffController : BaseApiController
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
            _baseService = (BaseService)staffService;
        }

        [HttpPost]
        [Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> CreateShopApplication([FromForm] CreateStaffRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _staffService.AddStaff(request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
        [HttpDelete]
        [Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> DeleteStaff(Guid id)
        {
            return await ExecuteServiceLogic(
                async () => await _staffService.DeleteStaff(id).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
        [HttpGet("GetListStaff")]
        [Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> GetListStaff(SearchStaffRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _staffService.GetListStaff(request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetStaffById{id}")]
        [Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> GetStaffById(Guid id)
        {
            return await ExecuteServiceLogic(
                async () => await _staffService.GetStaffById(id).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetTotalStaff")]
        [Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> GetTotalStaff()
        {
            return await ExecuteServiceLogic(
                async () => await _staffService.GetTotalStaff().ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
    }
}
