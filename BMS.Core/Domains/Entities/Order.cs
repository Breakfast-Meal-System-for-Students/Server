using BMS.Core.Domains.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class Order : EntityBase<Guid>
    {
      
        public double TotalPrice { get; set; }
        public string Status { get; set; } = null!;

        public Guid CustomerId { get; set; }
        public User Customer { get; set; } = null!;

        public Guid ShopId { get; set; }
        public Shop Shop { get; set; } = null!;

        public string QRCode { get; set; }
        public DateTime? OrderDate { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public Guid? FeedbackId { get; set; }
        public Feedback? Feedback { get; set; }
        public bool IsGroup { get; set; }
        public string? ReasonOfCancel {  get; set; }
    }

}
