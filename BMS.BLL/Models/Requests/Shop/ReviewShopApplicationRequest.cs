﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Shop
{
    public class ReviewShopApplicationRequest
    {
        [Required]
        public Guid Id {  get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
