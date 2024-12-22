using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.Wallet
{
    public class WalletTransactionResponse
    {
        public Guid WalletID { get; set; }
        public double Price { get; set; }
        public TransactionStatus Status { get; set; }
    }
}
