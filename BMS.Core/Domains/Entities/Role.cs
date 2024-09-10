using BMS.Core.Domains.Entities.BaseEntities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public string Name { get; set; } = "null"!;

        public ICollection<UserRole> UserRoles { get; set; }
    }
}
