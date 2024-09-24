using BMS.Core.Domains.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BMS.Core.Domains.Entities
{
    public class Product : EntityBase<Guid>, ISoftDelete
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }

      
        public Guid ShopId { get; set; }
        public Shop Shop { get; set; } = null!;

        public ICollection<Image>? Images { get; set; } = new List<Image>();
        public ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();

        public ICollection<CartDetail>? CartDetails { get; set; } = new List<CartDetail>();

        public ICollection<RegisterCategory>? RegisterCategorys { get; set; } = new List<RegisterCategory>();
        // Implement ISoftDelete properties
        public bool IsDeleted { get; set; } = false; // Default value here
        public DateTime? DeletedDate { get; set; }
    }
}
