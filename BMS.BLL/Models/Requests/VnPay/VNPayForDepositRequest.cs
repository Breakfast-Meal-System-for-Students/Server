using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.VnPay
{
    public class VNPayForDepositRequest
    {
        public string UserId { get; set; }
        public string OrderType { get; set; }
        public double Amount { get; set; }
        public string ReturnUrl { get; set; }
    }
}
