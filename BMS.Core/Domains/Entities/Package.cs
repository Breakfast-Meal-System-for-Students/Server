using BMS.Core.Domains.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class Package : EntityBase<Guid>,ISoftDelete
    {
        public string Name { get; set; } = null!;
        public double Price { get; set; }
        public string Description { get; set; } = null!;
        public int Duration { get; set; }

        public ICollection<Package_Shop> Package_Shop { get; set; } = new List<Package_Shop>();
        // Implement ISoftDelete properties
        public bool IsDeleted { get; set; } = false; // Default value here
        public DateTime? DeletedDate { get; set; }
    }

}
