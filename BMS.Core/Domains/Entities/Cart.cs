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

        public Guid ShopId { get; set; }
        public Shop Shop { get; set; } = null!;
        public bool IsGroup { get; set; }

        // Quan hệ 1-n với CartDetail
        public ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();
        public ICollection<CartGroupUser> CartGroupUsers { get; set; } = new List<CartGroupUser>();
    }

}
