using BMS.Core.Domains.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class CartGroupUser : EntityBase<Guid>
    {
        public Guid CartId { get; set; }
        public Cart Cart { get; set; } = null!;
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();
    }
}
