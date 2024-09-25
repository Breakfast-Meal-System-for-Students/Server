using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.VnPay
{
    public class VnPayRequest
    {
        public string OrderInfo { get; set; }
        public string FullName { get; set; }
        public string OrderType { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
    }
}
