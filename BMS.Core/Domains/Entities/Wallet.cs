using BMS.Core.Domains.Entities.BaseEntities;
using System;
using System.Collections.Generic;

namespace BMS.Core.Domains.Entities
{
    public class Wallet : EntityBase<Guid>, ISoftDelete
    {
        // Properties
        public string WalletName { get; set; } = "BMS Wallet"; // Default wallet name
        public decimal Balance { get; set; } = 0; // Current balance
        public Guid UserId { get; set; } // Reference to the user
        public User User { get; set; } = null!; // Navigation property

        public ICollection<WalletTransaction> WalletTransactions { get; set; } = new List<WalletTransaction>();

        // Soft delete properties
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
    }
}
