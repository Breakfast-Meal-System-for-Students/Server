using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.VnPay
{
    public class VnPayResponse
    {
        public string? vnp_Amount { get; set; }
        public string? vnp_OrderInfo { get; set; }
        public string? vnp_ResponseCode { get; set; }

    }
}
