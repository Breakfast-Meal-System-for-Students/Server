using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.Package;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{

    public class PackageController : BaseApiController
    {
        private readonly IPackageService _packageService;



        public PackageController(IPackageService PackageService)
        {
            _packageService = PackageService;



            _baseService = (BaseService)_packageService;
        }


        [HttpDelete("{id}")]

        public async Task<IActionResult> DeletePackage(Guid id)
        {

            return await ExecuteServiceLogic(
                               async () => await _packageService.DeletePackage(id).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePackage([FromBody] CreatePackageRequest Package)
        {
            return await ExecuteServiceLogic(
                               async () => await _packageService.AddPackage(Package).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPackage([FromQuery] PackageRequest pagingRequest)
        {
            return await ExecuteServiceLogic(
                               async () => await _packageService.GetAllPackage(pagingRequest).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackage(Guid Id, [FromBody] UpdatePackageRequest Package)
        {
            return await ExecuteServiceLogic(
                               async () => await _packageService.UpdatePackage(Id, Package).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackage(Guid id)
        {
            return await ExecuteServiceLogic(
                async () => await _packageService.GetPackage(id).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetPackageForShopInUse/{shopId}")]
        public async Task<IActionResult> GetPackageForShopInUse(Guid shopId, [FromQuery]PackageRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _packageService.GetPackageForShopInUse(shopId, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetPackageForHistoryBuyingByShop/{shopId}")]
        public async Task<IActionResult> GetPackageForHistoryBuyingByShop(Guid shopId, [FromQuery] PackageRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _packageService.GetPackageForHistoryBuyingByShop(shopId, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpPost("BuyPackageByShop")]
        public async Task<IActionResult> BuyPackageByShop(Guid shopId, Guid packageId)
        {
            return await ExecuteServiceLogic(
                async () => await _packageService.BuyPackageByShop(shopId, packageId).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
    }
}
