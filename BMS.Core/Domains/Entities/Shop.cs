using BMS.Core.Domains.Entities.BaseEntities;
using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;

namespace BMS.Core.Domains.Entities
{
    public class Shop : EntityBase<Guid>, ISoftDelete
    {
        // Properties
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

        // Relationships
        public Guid? UserId { get; set; }
        public User? User { get; set; } = null!;

        public ICollection<ShopUniversity> ShopUniversities { get; set; } = new List<ShopUniversity>();

        // Navigation Properties
        public ICollection<Product>? Products { get; set; } = new List<Product>();
        public ICollection<Coupon>? Coupons { get; set; } = new List<Coupon>();
        public ICollection<Order>? Orders { get; set; } = new List<Order>();
        public ICollection<Cart>? Carts { get; set; } = new List<Cart>();
        public ICollection<Package_Shop>? Package_Shop { get; set; } = new List<Package_Shop>();
        public ICollection<Feedback>? Feedbacks { get; set; } = new List<Feedback>();
        public ICollection<Notification>? Notifications { get; set; } = new List<Notification>();
        public ICollection<OpeningHours>? OpeningHours { get; set; } = new List<OpeningHours>();
        public ICollection<ShopWeeklyReport>? ShopWeeklyReports { get; set; } = new List<ShopWeeklyReport>();
        public ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
    }
}
