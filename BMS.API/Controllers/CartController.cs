﻿using BMS.API.Controllers.Base;
using BMS.API.Hub;
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
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BMS.API.Controllers
{
    public class CartController : BaseApiController
    {
        private readonly ICartService _cartService;
        private readonly IUserClaimsService _userClaimsService;
        private UserClaims _userClaims;
        private readonly IHubContext<CartHub> _hubContext;
        private readonly ITokenService _tokenService;

        public CartController(ICartService cartService, IUserClaimsService userClaimsService, IHubContext<CartHub> hubContext, ITokenService tokenService)
        {
            _cartService = cartService;
            _userClaimsService = userClaimsService;
            _userClaims = userClaimsService.GetUserClaims();
            _baseService = (BaseService)cartService;
            _hubContext = hubContext;
            _tokenService = tokenService;
        }

        [HttpGet("GetAllCartForUser")]
        [Authorize]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> GetAllCartForUser([FromQuery]PagingRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _cartService.GetAllCartForUser(_userClaims.UserId, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetAllCartItemInCart")]
        [Authorize]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> GetAllCartItemInCart(Guid cartId, [FromQuery]PagingRequest request)
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
            var result = await _cartService.DeleteCart(cartId).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                // Notify all clients with the same userId
                await _hubContext.Clients.Group(_userClaims.UserId.ToString())
                    .SendAsync("CartUpdated", _userClaims.UserId);
            }

            return await ExecuteServiceLogic(() => Task.FromResult(result)).ConfigureAwait(false);
        }

        [HttpDelete("DeleteCartItem")]
        [Authorize]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> DeleteCartItem([FromQuery]Guid cartItemId)
        {
            var result = await _cartService.DeleteCartItem(cartItemId).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                // Notify all clients with the same userId
                await _hubContext.Clients.Group(_userClaims.UserId.ToString())
                    .SendAsync("CartUpdated", _userClaims.UserId);
            }

            return await ExecuteServiceLogic(() => Task.FromResult(result)).ConfigureAwait(false);
        }

        [HttpPost("AddCartDetail")]
        [Authorize]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> AddCartDetail([FromBody]CartDetailRequest request)
        {
            var result = await _cartService.AddCartDetail(_userClaims.UserId, request.ShopId, request).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                await _hubContext.Clients.Group(_userClaims.UserId.ToString())
                    .SendAsync("CartAdded", _userClaims.UserId);
            }

            return await ExecuteServiceLogic(() => Task.FromResult(result)).ConfigureAwait(false);
        }

        [HttpPost("AddCartDetailForGroup")]
        [Authorize]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> AddCartDetailForGroup([FromBody] CartGroupDetailRequest request)
        {
            var result = await _cartService.AddCartDetailForGroup(_userClaims.UserId, request.CartId, request).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                await _hubContext.Clients.Group(_userClaims.UserId.ToString())
                    .SendAsync("CartGroupAdded", _userClaims.UserId);
            }

            return await ExecuteServiceLogic(() => Task.FromResult(result)).ConfigureAwait(false);
        }

        [HttpPost("UpdateCartDetail")]
        [Authorize]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> UpdateCartDetail([FromBody]CartDetailRequest request)
        {

            var result = await _cartService.UpdateCartDetail(_userClaims.UserId, request.ShopId, request).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                await _hubContext.Clients.Group(_userClaims.UserId.ToString())
                    .SendAsync("CartUpdated", _userClaims.UserId);
            }

            return await ExecuteServiceLogic(() => Task.FromResult(result)).ConfigureAwait(false);
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

        [HttpGet("GetCartInShopForUser")]
        [Authorize]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> GetCartInShopForUser([FromQuery] Guid shopId)
        {
            return await ExecuteServiceLogic(
                async () => await _cartService.GetCartInShopForUser(_userClaims.UserId, shopId).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetCartByID/{cartId}")]
        [Authorize]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> GetCartByID(Guid cartId)
        {
            return await ExecuteServiceLogic(
                async () => await _cartService.GetCartByID(cartId).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetCartBySharing/{cartId}")]
        [Authorize]
        public async Task<IActionResult> GetCartBySharing(Guid cartId, [FromQuery] string access_token)
        {
            var cartIdFromToken = await _tokenService.CheckTokenForShareLink(access_token);

                if (cartIdFromToken == null || cartIdFromToken != cartId.ToString())
                {
                    return Unauthorized("Invalid access to the cart.");
                }

             return await ExecuteServiceLogic(
                    async () => await _cartService.GetCartByID(cartId).ConfigureAwait(false)
                ).ConfigureAwait(false);
         }


        [HttpPost("ChangeCartToGroup")]
        [Authorize]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> ChangeCartToGroup([FromForm]Guid shopId)
        {
            return await ExecuteServiceLogic(
                async () => await _cartService.ChangeCartToGroup(_userClaims.UserId, shopId).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("CountCartItemInShop")]
        [Authorize]
        //[Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> CountCartItemInShop([FromQuery] Guid shopId)
        {
            return await ExecuteServiceLogic(
                async () => await _cartService.CountCartItemInShop(_userClaims.UserId, shopId).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
    }
}
