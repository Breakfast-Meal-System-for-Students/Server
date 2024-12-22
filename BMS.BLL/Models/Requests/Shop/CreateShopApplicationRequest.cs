using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Shop
{
    public class CreateShopApplicationRequest
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public IFormFile? Image { get; set; }
        public string Address { get; set; } = null!;
      //  public double Rate { get; set; }

        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public Guid? UniversityId { get; set; } = null;
        public int to_hour { get; set; }
        public int to_minute { get; set; }
        public int from_hour { get; set; }
        public int from_minute { get; set; }


        // public ShopStatus Status { get; set; } = ShopStatus.PENDING;
    }
}
