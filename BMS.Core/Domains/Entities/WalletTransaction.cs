using BMS.Core.Domains.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class WalletTransaction : EntityBase<Guid>
    {
        public Guid WalletID { get; set; }
        public double Price { get; set; }
        public Wallet? Wallet { get; set; }
        
    }
}
