using BMS.Core.Domains.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class Package_Shop : EntityBase<Guid>
    {
        public Guid ShopId { get; set; }
        public Shop Shop { get; set; } = null!;

        public Guid PackageId { get; set; }
        public Package Package { get; set; } = null!;
        public double Price { get; set; }
        public int Duration {  get; set; }
    }
}
