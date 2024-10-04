﻿using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.Notification
{
    public class NotificationResponse
    {
        public string Object { get; set; } = null!;
        public NotificationStatus Status { get; set; } = NotificationStatus.UnRead;
        public NotificationTitle? Title { get; set; }
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ShopId { get; set; }
    }
}
