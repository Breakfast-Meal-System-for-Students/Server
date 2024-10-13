using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.OpeningHour;
using BMS.BLL.Models.Requests.ShopWeeklyReport;
using BMS.BLL.Services;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
    public class OpeningHourController : BaseApiController
    {
        private readonly IOpeningHoursService _openingHoursService;
        public OpeningHourController(IOpeningHoursService openingHoursService)
        {
            _openingHoursService = openingHoursService;
            _baseService = (BaseService)openingHoursService;
        }

        [HttpGet("GetOpeningHoursForShop")]
        //[Authorize]
        public async Task<IActionResult> GetOpeningHoursForShop([FromQuery] Guid shopId)
        {
            return await ExecuteServiceLogic(
                               async () => await _openingHoursService.GetOpeningHoursForShop(shopId).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpPut("UpdateOpeningHoursForShop")]
        //[Authorize]
        public async Task<IActionResult> UpdateOpeningHoursForShop([FromBody] UpdateOpeningHoursRequest request)
        {
            return await ExecuteServiceLogic(
                               async () => await _openingHoursService.UpdateOpeningHoursForShop(request).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
    }
}
