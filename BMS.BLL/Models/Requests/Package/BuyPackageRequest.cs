using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Package
{
    public class BuyPackageRequest
    {
        [Required]
        public Guid ShopId { get; set; }
        [Required]
        public Guid PackageId { get; set; }
    }
}
