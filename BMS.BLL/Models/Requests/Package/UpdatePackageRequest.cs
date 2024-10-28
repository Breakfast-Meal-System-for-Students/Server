using BMS.Core.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Package
{
    public class UpdatePackageRequest
    {
        public string Name { get; set; } = null!;
        public double Price { get; set; }
        public string Description { get; set; } = null!;
        public int Duration { get; set; }

        public ICollection<Package_Shop> PackageHistories { get; set; } = new List<Package_Shop>();
        // Implement ISoftDelete properties
        public bool IsDeleted { get; set; } = false; // Default value here
        public DateTime? DeletedDate { get; set; }
    }
}
