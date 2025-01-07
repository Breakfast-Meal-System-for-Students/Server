using BMS.BLL.Models.Requests.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Package
{
    public class GetAmountAndRevenueOfEachPackageRequest : PagingRequest
    {
        public int? Month { get; set; } = 0;
        public int? Year { get; set; } = 0;
    }
}
