﻿using BMS.Core.Domains.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Product
{
    public class CreateProductRequest
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }

        public Guid ShopId { get; set; }

        public bool? IsCombo { get; set; } = false;

        public List<IFormFile> Images { get; set; } = new List<IFormFile>();

    }
}
