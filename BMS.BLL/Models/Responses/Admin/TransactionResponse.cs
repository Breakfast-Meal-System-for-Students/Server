using BMS.Core.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BMS.BLL.Models.Responses.Admin
{
    public class TransactionResponse
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }

        public double Price { get; set; }

        public string Method { get; set; } = null!;

        public TransactionStatus Status { get; set; }

        public Order Order { get; set; }
    }
}
