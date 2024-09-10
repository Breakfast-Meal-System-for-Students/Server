using BMS.Core.Domains.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class Package : EntityBase<Guid>
    {
        public string Name { get; set; } = null!;
        public double Price { get; set; }
        public string Description { get; set; } = null!;
        public int Duration { get; set; }

        public ICollection<PackageHistory> PackageHistories { get; set; } = new List<PackageHistory>();
    }

}
