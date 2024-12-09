using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.Shop;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
 
    public class ShopController : BaseApiController
    {
        private readonly IShopService _shopService;



        public ShopController(IShopService ShopService)
        {
            _shopService = ShopService;
            _baseService = (BaseService)_shopService;
        }


        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteShop(Guid id)
        {

            return await ExecuteServiceLogic(
                               async () => await _shopService.DeleteShop(id).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShop([FromQuery] ShopRequest pagingRequest)
        {
            return await ExecuteServiceLogic(
                               async () => await _shopService.GetAllShop(pagingRequest).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShop(Guid Id, [FromForm] UpdateShopRequest Shop)
        {
            return await ExecuteServiceLogic(
                               async () => await _shopService.UpdateShop(Id, Shop).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpPut("UpdateShopByStaff/{id}")]
        public async Task<IActionResult> UpdateShopByStaff(Guid Id, [FromForm] UpdateShopRequestByStaff Shop)
        {
            return await ExecuteServiceLogic(
                               async () => await _shopService.UpdateShopByStaff(Id, Shop).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShop(Guid id)
        {
            return await ExecuteServiceLogic(
                async () => await _shopService.GetShop(id).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetAllShopForMobile")]
        public async Task<IActionResult> GetAllShopForMobile([FromQuery] ShopRequest pagingRequest)
        {
            return await ExecuteServiceLogic(
                               async () => await _shopService.GetAllShopForMobile(pagingRequest).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpGet("CountNewShop")]
        [Authorize(Roles = UserRoleConstants.ADMIN + "," + UserRoleConstants.STAFF)]
        public async Task<IActionResult> CountNewShop([FromQuery] TotalShopRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _shopService.CountNewShop(request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
    }
}
