using BMS.Core.Domains.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class OpeningHours : EntityBase<Guid>
    {
        public int day { get; set; }

        public int from_hour { get; set; }
        public int to_hour { get; set; }
        public int from_minute { get; set; }
        public int to_minute { get; set; }
        public Guid ShopId { get; set; }
        public Shop Shop { get; set; } = null!;
          }
}
