using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.OpeningHour
{
      public class CreateOpeningHoursRequest
    {
        public WeekDay day { get; set; }

        public int from_hour { get; set; }
        public int to_hour { get; set; }
        public int from_minute { get; set; }
        public int to_minute { get; set; }
        public Guid ShopId { get; set; }
    }
}
