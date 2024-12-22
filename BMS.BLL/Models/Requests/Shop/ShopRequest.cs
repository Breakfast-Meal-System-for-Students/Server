using BMS.BLL.Models.Requests.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Shop
{
    public class ShopRequest : PagingRequest
    {
        public string Status { get; set; } = string.Empty;
        public string? Search { get; set; }
        public bool IsDesc { get; set; } = false;
        public string? University { get; set; }
    }
}
