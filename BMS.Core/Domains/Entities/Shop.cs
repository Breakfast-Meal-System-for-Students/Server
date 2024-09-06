using BMS.Core.Domains.Entities.BaseEntities;
using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class Shop : EntityBase<Guid>
    {
   
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;
        public string Address { get; set; } = null!;
        public double Rate { get; set; }
        public ShopStatus Status { get; set; } = ShopStatus.PENDING;
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime LastUpdateDate { get; set; } = DateTime.Now;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<CategoryShop> CategoryShops { get; set; } = new List<CategoryShop>();
        public ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<PackageHistory> PackageHistories { get; set; } = new List<PackageHistory>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }

}
