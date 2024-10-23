using BMS.Core.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.OpeningHour
{
    public class UpdateOpeningHoursRequest
    {
        public Guid shopId {  get; set; }
        public List<OpeningHoursRequest> listOpeningHours { get; set; } = new List<OpeningHoursRequest>();
    }
}
