﻿using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.Admin;
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
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
            _baseService = (BaseService)orderService;
        }

        [HttpPost("GetListOrders")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> GetListOrders(SearchOrderRequest request)
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
        public async Task<IActionResult> GetOrderByShop(Guid id, SearchOrderRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _orderService.GetOrderByShop(id, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetOrderByUser")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> GetOrderByUser(Guid id, SearchOrderRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _orderService.GetOrderByUser(id, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetTotalOrder")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> GetTotalOrder(TotalOrdersRequest request)
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
    }
}