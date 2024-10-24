using Azure.Core;
using BMS.API.Controllers.Base;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Models.Requests.Notification;
using BMS.BLL.Services;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Entities;
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

        [HttpPut("ClearAllNotification")]
        [Authorize]
        public async Task<IActionResult> ClearAllNotification()
        {
            return await ExecuteServiceLogic(
                async () => await _notificationService.ClearNotification(_userClaims.UserId).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
    }
}
