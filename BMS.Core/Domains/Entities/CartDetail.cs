using BMS.Core.Domains.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class CartDetail : EntityBase<Guid>
    {
        public Guid CartId { get; set; } // Khóa ngoại tới Cart
        public Cart Cart { get; set; } = null!; // Quan hệ n-1 với Cart

        public Guid ProductId { get; set; } // Khóa ngoại tới Product
        public Product Product { get; set; } = null!; // Quan hệ n-1 với Product

        public Guid? CartGroupUserId { get; set; }
        public CartGroupUser CartGroupUser { get; set; } = null!;
        public int Quantity { get; set; }
        public double Price { get; set; }
    }

}
