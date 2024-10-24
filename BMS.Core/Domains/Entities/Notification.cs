using BMS.Core.Domains.Entities.BaseEntities;
using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class Notification : EntityBase<Guid>, ISoftDelete
    {
        public string Object { get; set; } = null!;
        public NotificationStatus Status { get; set; } = NotificationStatus.UnRead;
        public NotificationTitle? Title { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public Guid ShopId { get; set; }
        public Shop Shop { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
    }

}
