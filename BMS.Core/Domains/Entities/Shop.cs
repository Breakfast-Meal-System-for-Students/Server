using BMS.Core.Domains.Entities.BaseEntities;
using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class Shop : EntityBase<Guid>, ISoftDelete
    {
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public string? Image { get; set; } = null!;
        public string Address { get; set; } = null!;
        public double? Rate { get; set; }
        public double? lat { get; set; }
        public double? lng { get; set; }
        public ShopStatus Status { get; set; } = ShopStatus.PENDING;

        public Guid? UserId { get; set; }
        public User? User { get; set; } = null!;

        public ICollection<Product>? Products { get; set; } = new List<Product>();
        public ICollection<Coupon>? Coupons { get; set; } = new List<Coupon>();
        public ICollection<Order>? Orders { get; set; } = new List<Order>();
        public ICollection<Cart>? Carts { get; set; } = new List<Cart>();
        public ICollection<PackageHistory>? PackageHistories { get; set; } = new List<PackageHistory>();
        public ICollection<Feedback>? Feedbacks { get; set; } = new List<Feedback>();
        public ICollection<Notification>? Notifications { get; set; } = new List<Notification>();
        public ICollection<OpeningHours>? OpeningHours { get; set; } = new List<OpeningHours>();

        public ICollection<ShopWeeklyReport>? ShopWeeklyReports { get; set; } = new List<ShopWeeklyReport>();
        //    public ICollection<RegisterCategory>? RegisterCategorys { get; set; } = new List<RegisterCategory>();

        public bool IsDeleted { get; set; } = false; // Default value here
        public DateTime? DeletedDate { get; set; }
    }

}
