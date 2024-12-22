﻿using BMS.BLL.Models.Requests.Basic;
using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Order
{
    public class GetOrdersInTimeRequest : PagingRequest
    {
        public OrderStatus Status { get; set; } = 0;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public bool IsDesc { get; set; } = false;
    }
}