using Azure.Core;
using BMS.API.Controllers.Base;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Models.Requests.Notification;
using BMS.BLL.Services;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BMS.API.Controllers
{
    public class NotificationController : BaseApiController
    {
        private readonly INotificationService _notificationService;
        private UserClaims _userClaims;
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationController(INotificationService notificationService, IUserClaimsService userClaimsService, IHubContext<NotificationHub> hubContext)
        {
            _notificationService = notificationService;
            _baseService = (BaseService)notificationService;
            _userClaims = userClaimsService.GetUserClaims();
            _hubContext = hubContext;
        }

        [HttpGet("GetNotificationForStaff")]
        [Authorize]
        public async Task<IActionResult> GetNotificationForStaff([FromQuery] GetNotificationRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _notificationService.GetNotificationForStaff(request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetNotificationForShop")]
        [Authorize]
        public async Task<IActionResult> GetNotificationForShop(Guid shopId, [FromQuery] GetNotificationRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _notificationService.GetNotificationForShop(shopId, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("CountNotificationForShop")]
        [Authorize]
        public async Task<IActionResult> CountNotificationForShop(Guid shopId)
        {
            return await ExecuteServiceLogic(
                async () => await _notificationService.CountNotificationForShop(shopId).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetNotificationForUser")]
        [Authorize]
        public async Task<IActionResult> GetNotificationForUser([FromQuery] GetNotificationRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _notificationService.GetNotificationForUser(_userClaims.UserId, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("CountNotificationForUser")]
        [Authorize]
        public async Task<IActionResult> CountNotificationForUser()
        {
            return await ExecuteServiceLogic(
                async () => await _notificationService.CountNotificationForUser(_userClaims.UserId).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpPut("ChangeStatusNotification")]
        [Authorize]
        public async Task<IActionResult> ChangeStatusNotification([FromForm] Guid notificationId)
        {
            var result = await _notificationService.ChangeStatusNotification(_userClaims.UserId, notificationId).ConfigureAwait(false);
            
            if (result.IsSuccess)
            {
                var notification = (Notification)result.Data;
                await _hubContext.Clients.User(notification.UserId.ToString()).SendAsync("Change Status Notification", notification.Object);
            }

            return await ExecuteServiceLogic(() => Task.FromResult(result)).ConfigureAwait(false);
        }

        [HttpPut("ClearNotificationForUser")]
        [Authorize]
        public async Task<IActionResult> ClearNotificationForUser([FromForm]NotificationStatus status)
        {
            ClearNotificationRequest request = new ClearNotificationRequest()
            {
                Id = _userClaims.UserId,
                Status = status
            };
            return await ExecuteServiceLogic(
                async () => await _notificationService.ClearNotificationForUser(request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpPut("ClearNotificationForShop")]
        [Authorize]
        public async Task<IActionResult> ClearNotificationForShop(ClearNotificationRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _notificationService.ClearNotificationForShop(request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
    }
}
