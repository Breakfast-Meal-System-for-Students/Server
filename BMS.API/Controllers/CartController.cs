using BMS.API.Controllers.Base;
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

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
            _baseService = (BaseService)cartService;
        }

        [HttpGet("GetAllCartForUser")]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> GetAllCartForUser(Guid userId, PagingRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _cartService.GetAllCartForUser(userId, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetAllCartItemInCart")]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> GetAllCartItemInCart(Guid cartId, PagingRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _cartService.GetAllCartItemInCart(cartId, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpDelete("DeleteCart")]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> DeleteCart(Guid cartId)
        {
            return await ExecuteServiceLogic(
                async () => await _cartService.DeleteCart(cartId).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpDelete("DeleteCartItem")]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> DeleteCartItem(Guid cartItemId)
        {
            return await ExecuteServiceLogic(
                async () => await _cartService.DeleteCartItem(cartItemId).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpPost("AddCartDetail")]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> AddCartDetail(Guid userId, Guid shopId, CartDetailRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _cartService.AddCartDetail(userId, shopId, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpPost("UpdateCartDetail")]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> UpdateCartDetail(Guid userId, Guid shopId, CartDetailRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _cartService.UpdateCartDetail(userId, shopId, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetCartDetail")]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> GetCartDetail(Guid cartDetailId)
        {
            return await ExecuteServiceLogic(
                async () => await _cartService.GetCartDetail(cartDetailId).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
    }
}
