using BMS.Core.Domains.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class Cart : EntityBase<Guid>
    {
        public Guid CustomerId { get; set; } 
        public User Customer { get; set; } = null!;

        
        public bool IsPurchase { get; set; }

        // Quan hệ 1-n với CartDetail
        public ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();
    }

}
