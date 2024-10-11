using BMS.BLL.Models.Requests.Basic;
using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Notification
{
    public class GetNotificationRequest : PagingRequest
    {
        public NotificationStatus Status { get; set; } = 0;
        public NotificationTitle Title { get; set; } = 0;
    }
}
