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
       // public ShopStatus Status { get; set; } = ShopStatus.PENDING;
    }
}
