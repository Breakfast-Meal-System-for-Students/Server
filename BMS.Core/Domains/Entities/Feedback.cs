using BMS.Core.Domains.Entities.BaseEntities;
using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class Feedback : EntityBase<Guid>
    {
        public string Description { get; set; } = null!;
        public int Rate { get; set; }

        public FeedbackStatus Status { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid ShopId { get; set; }
        public Shop Shop { get; set; } = null!;

        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
    }

}
