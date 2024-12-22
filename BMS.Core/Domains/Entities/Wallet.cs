using BMS.Core.Domains.Entities.BaseEntities;
using System;
using System.Collections.Generic;

namespace BMS.Core.Domains.Entities
{
    public class Wallet : EntityBase<Guid>, ISoftDelete
    {
        // Properties
        public string WalletName { get; set; } = "BMS Wallet";
        public decimal Balance { get; set; } = 0;
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<WalletTransaction> WalletTransactions { get; set; } = new List<WalletTransaction>();

        
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
    }
}
