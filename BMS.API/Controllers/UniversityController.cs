using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.University;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{

    public class UniversityController : BaseApiController
    {
        private readonly IUniversityService _packageService;



        public UniversityController(IUniversityService UniversityService)
        {
            _packageService = UniversityService;
            _baseService = (BaseService)_packageService;
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoleConstants.ADMIN + "," + UserRoleConstants.STAFF)]
        public async Task<IActionResult> DeleteUniversity(Guid id)
        {

            return await ExecuteServiceLogic(
                               async () => await _packageService.DeleteUniversity(id).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpPost]
        [Authorize(Roles = UserRoleConstants.ADMIN + "," + UserRoleConstants.STAFF)]
        public async Task<IActionResult> CreateUniversity([FromBody] CreateUniversityRequest University)
        {
            return await ExecuteServiceLogic(
                               async () => await _packageService.AddUniversity(University).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUniversity([FromQuery] UniversityRequest pagingRequest)
        {
            return await ExecuteServiceLogic(
                               async () => await _packageService.GetAllUniversity(pagingRequest).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = UserRoleConstants.ADMIN + "," + UserRoleConstants.STAFF)]
        public async Task<IActionResult> UpdateUniversity(Guid Id, [FromBody] UpdateUniversityRequest University)
        {
            return await ExecuteServiceLogic(
                               async () => await _packageService.UpdateUniversity(Id, University).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUniversity(Guid id)
        {
            return await ExecuteServiceLogic(
                async () => await _packageService.GetUniversity(id).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }


    }
}
