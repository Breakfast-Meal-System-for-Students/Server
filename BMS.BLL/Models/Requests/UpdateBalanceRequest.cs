using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests
{
    public class UpdateBalanceRequest
    {
        public TransactionStatus Status { get; set; }
        public decimal Amount { get; set; }
        public Guid? OrderId { get; set; } = null!;
    }
}
