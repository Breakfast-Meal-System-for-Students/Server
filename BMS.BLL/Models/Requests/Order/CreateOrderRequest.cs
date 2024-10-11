using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Order
{
    public class CreateOrderRequest
    {
        public Guid CartId { get; set; }
        public Guid VoucherId { get; set; } = Guid.Empty;
        public DateTime? OrderDate { get; set; }
    }
}
