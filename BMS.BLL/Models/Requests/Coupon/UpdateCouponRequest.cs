using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Coupon
{
    public class UpdateCouponRequest
    {
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double PercentDiscount { get; set; }
        public double MaxDiscount { get; set; }
        public double MinPrice { get; set; }
        public double MinDiscount { get; set; }

        //public Guid ShopId { get; set; }



    }
}
