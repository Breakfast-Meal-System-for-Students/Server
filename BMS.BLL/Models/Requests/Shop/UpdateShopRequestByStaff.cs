﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Shop
{
    public class UpdateShopRequestByStaff
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public IFormFile? Image { get; set; }
        public string Address { get; set; } = null!;
        public double? lat { get; set; }
        public double? lng { get; set; }
        public string Phone { get; set; } = null!;
    }
}
