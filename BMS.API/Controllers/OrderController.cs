using BMS.API.Controllers.Base;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Models.Requests.Category;
using BMS.BLL.Services;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Constants;
using BMS.Core.Domains.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IUserClaimsService _userClaimsService;
        private UserClaims _userClaims;
        public OrderController(IOrderService orderService, IUserClaimsService userClaimsService)
        {
            _orderService = orderService;
            _baseService = (BaseService)orderService;
            _userClaimsService = userClaimsService;
            _userClaims = userClaimsService.GetUserClaims();
        }

        [HttpGet("GetListOrders")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> GetListOrders([FromQuery]SearchOrderRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _orderService.GetListOrders(request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetOrderById{id}")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            return await ExecuteServiceLogic(
                async () => await _orderService.GetOrderByID(id).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetOrderByShop")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> GetOrderByShop(Guid id, [FromQuery]SearchOrderRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _orderService.GetOrderByShop(id, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetOrderByUser")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> GetOrderByUser(Guid id, [FromQuery] SearchOrderRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _orderService.GetOrderByUser(id, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetOrderForUser")]
        [Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> GetOrderForUser([FromQuery] SearchOrderRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _orderService.GetOrderByUser(_userClaims.UserId, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetTotalOrder")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> GetTotalOrder([FromQuery] TotalOrdersRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _orderService.GetTotalOrder(request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpPost("ChangeOrderStatus")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> ChangeOrderStatus(Guid id, OrderStatus status)
        {
            return await ExecuteServiceLogic(
                async () => await _orderService.ChangeOrderStatus(id, status).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpPost("CreateOrder")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> CreateOrder(Guid cartId, Guid voucherId)
        {
            return await ExecuteServiceLogic(
                async () => await _orderService.CreateOrder(cartId, voucherId).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetStatusOrder")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> GetStatusOrder([FromQuery] Guid orderId)
        {
            return await ExecuteServiceLogic(
                async () => await _orderService.GetStatusOrder(orderId).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
        [HttpPut("{id}")]
        //[Authorize(Roles = UserRoleConstants.SHOP)]
        public async Task<IActionResult> UpdateStatusOrder(Guid Id, string status)
        {
            return await ExecuteServiceLogic(
                               async () => await _orderService.UpdateStatusOrder(Id, status).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpGet("CheckOrderIsPayed{orderId}")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> CheckOrderIsPayed(Guid orderId)
        {
            return await ExecuteServiceLogic(
                async () => await _orderService.CheckOrderIsPayed(orderId).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
    }
}
