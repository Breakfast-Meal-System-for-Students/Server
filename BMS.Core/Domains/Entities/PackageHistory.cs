using BMS.Core.Domains.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
  public class PackageHistory : EntityBase<Guid>
    {
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public DateTime LastUpdateDate { get; set; } = DateTime.Now;

    public Guid ShopId { get; set; }
    public Shop Shop { get; set; } = null!;
    
    public Guid PackageId { get; set; }
    public Package Package { get; set; } = null!;
}

}
