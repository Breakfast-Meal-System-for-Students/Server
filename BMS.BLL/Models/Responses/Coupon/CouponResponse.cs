using BMS.Core.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.Coupon
{
    public class CouponResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double PercentDiscount { get; set; }
        public double MaxDiscount { get; set; }
        public double MinPrice { get; set; }
        public double MinDiscount { get; set; }
        public bool isPercentDiscount { get; set; }

        public Guid ShopId { get; set; }
        

        // Implement ISoftDelete properties
        public bool IsDeleted { get; set; } = false; // Default value here
        public DateTime? DeletedDate { get; set; }
    }
}
