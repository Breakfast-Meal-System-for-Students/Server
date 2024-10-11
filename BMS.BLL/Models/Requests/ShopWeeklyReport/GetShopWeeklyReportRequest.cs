using BMS.BLL.Models.Requests.Basic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.ShopWeeklyReport
{
    public class GetShopWeeklyReportRequest : PagingRequest
    {
        public int? Day { get; set; } = 0;
        public int? Month { get; set; } = 0;
        public int? Year { get; set; } = 0;
        public Guid? ShopId { get; set; }
        public string? ShopName { get; set; }
        public bool IsDesc { get; set; } = false;
    }
}
