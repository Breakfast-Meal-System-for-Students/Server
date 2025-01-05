using BMS.Core.Domains.Entities.BaseEntities;
using System;

namespace BMS.Core.Domains.Entities
{
    public class ShopUniversity : EntityBase<Guid>
    {
        public Guid ShopId { get; set; }
        public Shop Shop { get; set; } = null!;

        public Guid UniversityId { get; set; }
        public University University { get; set; } = null!;
    }
}
