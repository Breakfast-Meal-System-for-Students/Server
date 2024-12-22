using BMS.Core.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.Wallet
{
    public class WalletResponse
    {
        public string WalletName { get; set; } = "BMS Wallet";
        public decimal Balance { get; set; } = 0;
        public Guid UserId { get; set; }
        public ICollection<WalletTransactionResponse> WalletTransactions { get; set; } = new List<WalletTransactionResponse>();
    }
}
