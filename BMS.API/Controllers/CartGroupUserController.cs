using BMS.API.Controllers.Base;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services;
using BMS.BLL.Services.IServices;
using BMS.BLL.Models.Requests.Basic;
using BMS.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
    public class CartGroupUserController : BaseApiController
    {
        private readonly ICartGroupUserService _cartGroupUserService;
        public CartGroupUserController(ICartGroupUserService cartGroupUserService) 
        {
            _cartGroupUserService = cartGroupUserService;
            _baseService = (BaseService)cartGroupUserService;
        }

        [HttpGet("GetAllUserInGroupCart")]
        //[Authorize]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> GetAllUserInGroupCart([FromQuery]Guid cartId, [FromQuery]PagingRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _cartGroupUserService.GetAllUserInGroupCart(cartId, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
    }
}
