using BMS.BLL.Models.Requests.Basic;
using BMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Product
{
    public class ProductBestSellerRequest : PagingRequest
    {
        public int? Day { get; set; } = 0;
        public int? Month { get; set; } = 0;
        public int? Year { get; set; } = 0;
        public bool isDesc { get; set; } = true;
    }
}
