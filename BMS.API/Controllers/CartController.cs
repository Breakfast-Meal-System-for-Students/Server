using BMS.API.Controllers.Base;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Models.Requests.Basic;
using BMS.BLL.Models.Requests.Cart;
using BMS.BLL.Services;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Constants;
using BMS.Core.Domains.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
    public class CartController : BaseApiController
    {
        private readonly ICartService _cartService;
        private readonly IUserClaimsService _userClaimsService;
        private UserClaims _userClaims;

        public CartController(ICartService cartService, IUserClaimsService userClaimsService)
        {
            _cartService = cartService;
            _userClaimsService = userClaimsService;
            _userClaims = userClaimsService.GetUserClaims();
            _baseService = (BaseService)cartService;
        }

        [HttpGet("GetAllCartForUser")]
        [Authorize]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> GetAllCartForUser([FromForm]PagingRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _cartService.GetAllCartForUser(_userClaims.UserId, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetAllCartItemInCart")]
        [Authorize]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> GetAllCartItemInCart(Guid cartId, [FromForm]PagingRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _cartService.GetAllCartItemInCart(cartId, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpDelete("DeleteCart")]
        [Authorize]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> DeleteCart([FromQuery]Guid cartId)
        {
            return await ExecuteServiceLogic(
                async () => await _cartService.DeleteCart(cartId).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpDelete("DeleteCartItem")]
        [Authorize]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> DeleteCartItem([FromQuery]Guid cartItemId)
        {
            return await ExecuteServiceLogic(
                async () => await _cartService.DeleteCartItem(cartItemId).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpPost("AddCartDetail")]
        [Authorize]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> AddCartDetail(Guid shopId, [FromBody]CartDetailRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _cartService.AddCartDetail(_userClaims.UserId, shopId, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpPost("UpdateCartDetail")]
        [Authorize]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> UpdateCartDetail(Guid userId, Guid shopId, [FromBody]CartDetailRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _cartService.UpdateCartDetail(userId, shopId, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetCartDetail")]
        [Authorize]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> GetCartDetail([FromQuery] Guid cartDetailId)
        {
            return await ExecuteServiceLogic(
                async () => await _cartService.GetCartDetail(cartDetailId).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
    }
}
