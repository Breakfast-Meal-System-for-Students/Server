using BMS.Core.Domains.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class ShopWeeklyReport : EntityBase<Guid>
    {
        public Guid ShopId { get; set; }
        public byte[] ReportData { get; set; }
        public Shop Shop { get; set; }
    }
}
