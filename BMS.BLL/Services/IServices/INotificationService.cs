using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Notification;
using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.IServices
{
    public interface INotificationService
    {
        Task<List<Notification>> GetAllNotificationsToSendMail(NotificationStatus status);
        Task<List<Notification>> GetAllNotificationsToSendNoti(Order order);
        Task SaveChange();
        Task<ServiceActionResult> GetNotificationForShop(Guid shopId, GetNotificationRequest request);
        Task<ServiceActionResult> CountNotificationForShop(Guid shopId);
        Task<ServiceActionResult> CountNotificationForUser(Guid userId);
        Task<ServiceActionResult> GetNotificationForUser(Guid userId, GetNotificationRequest request);
        Task<ServiceActionResult> ChangeStatusNotification(Guid userId, Guid notificationId);

        Task<ServiceActionResult> CreateNotification(Order order);
        Task<ServiceActionResult> ClearNotificationForUser(ClearNotificationRequest request);
        Task<ServiceActionResult> ClearNotificationForShop(ClearNotificationRequest request);
        Task<ServiceActionResult> ReadAllNotificationForUser(Guid userId);
        Task<ServiceActionResult> ReadAllNotificationForShop(Guid shopId);
        Task<ServiceActionResult> GetNotificationForStaff(GetNotificationRequest request);
    }
}
