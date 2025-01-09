using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Order
{
    public class ChangeOrderStatusRequest
    {
        public Guid Id { get; set; }
        public OrderStatus Status { get; set; }
        public string? ReasonOfCancel {  get; set; }
    }
}
