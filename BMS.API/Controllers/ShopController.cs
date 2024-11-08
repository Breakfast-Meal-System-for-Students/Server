using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.Shop;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
 
    public class ShopController : BaseApiController
    {
        private readonly IShopService _packageService;



        public ShopController(IShopService ShopService)
        {
            _packageService = ShopService;
            _baseService = (BaseService)_packageService;
        }


        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteShop(Guid id)
        {

            return await ExecuteServiceLogic(
                               async () => await _packageService.DeleteShop(id).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllShop([FromQuery] ShopRequest pagingRequest)
        {
            return await ExecuteServiceLogic(
                               async () => await _packageService.GetAllShop(pagingRequest).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShop(Guid Id, [FromForm] UpdateShopRequest Shop)
        {
            return await ExecuteServiceLogic(
                               async () => await _packageService.UpdateShop(Id, Shop).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpGet("{id}")]


        public async Task<IActionResult> GetShop(Guid id)
        {
            return await ExecuteServiceLogic(
                async () => await _packageService.GetShop(id).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
    }
}
