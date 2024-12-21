using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.StudentApplication;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{

    public class StudentApplicationController : BaseApiController
    {
        private readonly IStudentApplicationService _studentApplicationService;



        public StudentApplicationController(IStudentApplicationService StudentApplicationService)
        {
            _studentApplicationService = StudentApplicationService;
            _baseService = (BaseService)_studentApplicationService;
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoleConstants.ADMIN + "," + UserRoleConstants.STAFF)]
        public async Task<IActionResult> DeleteStudentApplication(Guid id)
        {

            return await ExecuteServiceLogic(
                               async () => await _studentApplicationService.DeleteStudentApplication(id).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudentApplication([FromQuery] StudentApplicationRequest pagingRequest)
        {
            return await ExecuteServiceLogic(
                               async () => await _studentApplicationService.GetAllStudentApplication(pagingRequest).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = UserRoleConstants.ADMIN + "," + UserRoleConstants.STAFF)]
        public async Task<IActionResult> UpdateStudentApplication(Guid Id, [FromBody] UpdateStudentApplicationRequest StudentApplication)
        {
            return await ExecuteServiceLogic(
                               async () => await _studentApplicationService.UpdateStudentApplication(Id, StudentApplication).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentApplication(Guid id)
        {
            return await ExecuteServiceLogic(
                async () => await _studentApplicationService.GetStudentApplication(id).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }


    }
}
