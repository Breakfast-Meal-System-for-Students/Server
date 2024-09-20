using BMS.Core.Domains.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class Coupon : EntityBase<Guid> ,ISoftDelete
    {
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double PercentDiscount { get; set; }
        public double MaxDiscount { get; set; }
        public double MinPrice { get; set; }
        public double MinDiscount { get; set; }

        public Guid ShopId { get; set; }
        public Shop Shop { get; set; } = null!;

        public ICollection<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();

        // Implement ISoftDelete properties
        public bool IsDeleted { get; set; } = false; // Default value here
        public DateTime? DeletedDate { get; set; }
    }

}
