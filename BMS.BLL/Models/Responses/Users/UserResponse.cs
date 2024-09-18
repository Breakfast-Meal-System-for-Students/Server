using BMS.Core.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.Users
{
    public class UserResponse
    {
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
        public string? Avatar { get; set; }

        public string? Phone { get; set; } = null!;
        //  public string? Password { get; set; } = null!;
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime LastUpdateDate { get; set; } = DateTime.Now;

        public IList<string>? Role { get; set; } = null!;
        //  public ICollection<UserRole>? UserRoles { get; set; }


    }
}
