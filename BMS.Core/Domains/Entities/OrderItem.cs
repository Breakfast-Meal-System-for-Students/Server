using BMS.Core.Domains.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class OrderItem : EntityBase<Guid>
    {
        public int Quantity { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime LastUpdateDate { get; set; } = DateTime.Now;

        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
    }
}
