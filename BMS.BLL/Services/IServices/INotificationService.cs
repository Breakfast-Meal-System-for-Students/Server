using BMS.BLL.Models;
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
        Task SaveChange();
        Task<ServiceActionResult> GetNotificationForShop(Guid ShopId);
        Task<ServiceActionResult> GetNotificationForUser(Guid userId);
    }
}
