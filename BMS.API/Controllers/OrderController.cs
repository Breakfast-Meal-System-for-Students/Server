using BMS.API.Controllers.Base;
using BMS.API.Hub;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Models.Requests.Category;
using BMS.BLL.Models.Requests.Order;
using BMS.BLL.Services;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Constants;
using BMS.Core.Domains.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BMS.API.Controllers
{
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IUserClaimsService _userClaimsService;
        private UserClaims _userClaims;
        private readonly IHubContext<OrderHub> _hubContext;
        public OrderController(IOrderService orderService, IUserClaimsService userClaimsService, IHubContext<OrderHub> hubContext)
        {
            _orderService = orderService;
            _baseService = (BaseService)orderService;
            _userClaimsService = userClaimsService;
            _userClaims = userClaimsService.GetUserClaims();
            _hubContext = hubContext;
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
        public async Task<IActionResult> GetOrderById1(Guid id)
        {
            return await ExecuteServiceLogic(
                async () => await _orderService.GetOrderByID(id).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetOrderById/{id}")]
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> ChangeOrderStatus([FromForm]ChangeOrderStatusRequest request)
        {
            var result = await _orderService.ChangeOrderStatus(request.Id, request.Status).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                await _hubContext.Clients.Group(_userClaims.UserId.ToString())
                    .SendAsync("OrderUpdated", _userClaims.UserId);
            }

            return await ExecuteServiceLogic(() => Task.FromResult(result)).ConfigureAwait(false);
        }

        [HttpPost("CreateOrder")]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var result = await _orderService.CreateOrder(request).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                await _hubContext.Clients.Group(_userClaims.UserId.ToString())
                    .SendAsync("OrderCreate", _userClaims.UserId);
            }
            return await ExecuteServiceLogic(() => Task.FromResult(result)).ConfigureAwait(false);
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

        [HttpGet("CheckOrderIsPayed/{orderId}")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> CheckOrderIsPayed(Guid orderId)
        {
            return await ExecuteServiceLogic(
                async () => await _orderService.CheckOrderIsPayed(orderId).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpPost("CheckQRCodeOfUser")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> CheckQRCodeOfUser([FromForm] string QRcode)
        {
            return await ExecuteServiceLogic(
                async () => await _orderService.CheckQRCodeOfUser(QRcode, _userClaims.UserId).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
    }
}
