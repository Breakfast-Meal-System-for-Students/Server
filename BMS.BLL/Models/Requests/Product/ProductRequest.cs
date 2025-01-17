﻿using BMS.BLL.Models.Requests.Basic;
using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Product
{
    public class ProductRequest : PagingRequest
    {
        public string? Search { get; set; }
        public bool IsDesc { get; set; } = false;
        public bool? IsOutOfStock { get; set; } = null;
        public bool? IsCombo { get; set; } = null;
        public AIDetectStatus? IsAICanDetect { get; set; } = null;
    }
}
