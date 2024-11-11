using BMS.Core.Domains.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class OTP : EntityBase<Guid>
    {
        public string Otp { get; set; } = null!;
        public DateTime EndDate { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
