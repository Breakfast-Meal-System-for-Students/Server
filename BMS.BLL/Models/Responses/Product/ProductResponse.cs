﻿using BMS.BLL.Models.Responses.Category;
using BMS.BLL.Models.Responses.Image;
using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.Product
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public bool? IsCombo { get; set; }
        public Guid ShopId { get; set; }
        public bool isOutOfStock { get; set; }
        public AIDetectStatus isAICanDetect { get; set; }
        public ICollection<ImageResponse> Images { get; set; } = new List<ImageResponse>();

        public ICollection<CategoryResponse>? Categorys { get; } = new List<CategoryResponse>();
    }
}
