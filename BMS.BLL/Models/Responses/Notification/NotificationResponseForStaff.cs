using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.Notification
{
    public class NotificationResponseForStaff
    {
        public string Object { get; set; } = null!;
        public NotificationStatus Status { get; set; } = NotificationStatus.UnRead;
        public NotificationTitle? Title { get; set; }
        public Guid UserId { get; set; }
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
        public string? Avatar { get; set; }
        public Guid OrderId { get; set; }
        public Guid ShopId { get; set; }
        public string ShopName { get; set; } = null!;
        public string? ShopImage { get; set; } = null!;
    }
}
