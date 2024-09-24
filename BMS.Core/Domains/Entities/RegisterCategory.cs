using BMS.Core.Domains.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class RegisterCategory : EntityBase<Guid>
    {
        public Guid CategoryId { get; set; } // Foreign key to Category
        public Category? Category { get; set; } = null!;

        //public Guid ShopId { get; set; } // Foreign key to Shop
        //public Shop? Shop { get; set; } = null!;

        public Product? Product { get; set; } = null!;
        public Guid ProductId { get; set; }
    }
}
