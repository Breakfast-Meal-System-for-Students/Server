using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.Package
{
    public class PackageResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public double Price { get; set; }
        public string Description { get; set; } = null!;
        public int Duration { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public bool IsDeleted { get; set; } = false; // Default value here
        public DateTime? DeletedDate { get; set; }
    }
}
