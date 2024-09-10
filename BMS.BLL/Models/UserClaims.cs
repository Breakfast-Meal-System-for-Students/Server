using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models
{
    public class UserClaims
    {
        public Guid UserId { get; set; }
        public string? Email { get; set; }
    }
}
