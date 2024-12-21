using BMS.Core.Domains.Entities.BaseEntities;
using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class OpeningHours : EntityBase<Guid>
    {
        public WeekDay day { get; set; }

        public int from_hour { get; set; }
        public int to_hour { get; set; }
        public int from_minute { get; set; }
        public int to_minute { get; set; }
        public Guid ShopId { get; set; }
        public Shop Shop { get; set; } = null!;

        public bool isOpenToday { get; set; } = true;
        public void Set(int from_hour, int to_hour, int from_minute, int to_minute)
        {
            this.from_hour = from_hour;
            this.to_hour = to_hour;
            this.from_minute = from_minute;
            this.to_minute = to_minute;
        }
    }
}
