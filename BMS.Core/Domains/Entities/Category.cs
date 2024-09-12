using BMS.Core.Domains.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class Category : EntityBase<Guid>
    {
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
        public string Description { get; set; } = null!;


        public ICollection<RegisterCategory>? RegisterCategorys { get; set; } = new List<RegisterCategory>();
    }

}
