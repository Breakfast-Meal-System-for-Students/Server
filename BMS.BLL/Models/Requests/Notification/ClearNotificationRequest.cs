using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Notification
{
    public class ClearNotificationRequest
    {
        public Guid Id { get; set; }
        public NotificationStatus Status { get; set; } = 0;
    }
}
