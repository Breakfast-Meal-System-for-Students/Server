using BMS.Core.Domains.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class Image : EntityBase<Guid>
    {
        public string Url { get; set; } = null!;

        public Guid ProductId { get; set; }
        public Product? Product { get; set; } = null!;
    }

}
