using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Admin
{
    public class TotalOrdersRequest
    {
        public int? Day { get; set; } = 0;
        public int? Month { get; set; } = 0;
        public int? Year { get; set; } = 0;
        public OrderStatus? Status { get; set; } = 0;
    }
}
