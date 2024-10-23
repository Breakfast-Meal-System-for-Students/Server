using AutoMapper;
using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.ShopWeeklyReport;
using BMS.BLL.Services;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
    public class ShopWeeklyReportController : BaseApiController
    {
        private readonly IShopWeeklyReportService _shopWeeklyReportService;
        public ShopWeeklyReportController(IShopWeeklyReportService shopWeeklyReportService)
        {
            _shopWeeklyReportService = shopWeeklyReportService;
            _baseService = (BaseService)shopWeeklyReportService;
        }

        [HttpGet("GetAllShopWeeklyReport")]
        //[Authorize]
        public async Task<IActionResult> GetAllShopWeeklyReport([FromQuery]GetShopWeeklyReportRequest request)
        {

            return await ExecuteServiceLogic(
                               async () => await _shopWeeklyReportService.GetAllShopWeeklyReport(request).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
    }
}
