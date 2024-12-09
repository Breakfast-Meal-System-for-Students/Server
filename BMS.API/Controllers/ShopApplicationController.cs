using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.Shop;
using BMS.BLL.Services;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{

    public class ShopApplicationController : BaseApiController
    {
        private readonly IShopApplicationService _shopApplicationService;
        public ShopApplicationController(IShopApplicationService shopApplicationService)
        {
            _shopApplicationService = shopApplicationService;
            _baseService = (BaseService)_shopApplicationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateShopApplication([FromForm] CreateShopApplicationRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _shopApplicationService.CreateShopApplication(request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet]

    //    [Authorize(Roles = $"{UserRoleConstants.STAFF} , {UserRoleConstants.ADMIN}")]
        public async Task<IActionResult> GetAllApplications([FromQuery] ShopApplicationRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _shopApplicationService.GetAllApplications(request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("{id}")]

       // [Authorize(Roles = $"{UserRoleConstants.STAFF} , {UserRoleConstants.ADMIN}")]

        public async Task<IActionResult> GetApplication(Guid id)
        {
            return await ExecuteServiceLogic(
                async () => await _shopApplicationService.GetApplication(id).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpPut]
        // [Authorize(Roles = $"{UserRoleConstants.STAFF} , {UserRoleConstants.ADMIN}")]
        public async Task<IActionResult> ReviewedApplication([FromForm] ReviewShopApplicationRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _shopApplicationService.ReviewApplication(request.Id, request.Status, request.Message).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
    }
}
