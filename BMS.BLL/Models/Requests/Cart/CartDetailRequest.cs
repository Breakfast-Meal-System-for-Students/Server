using BMS.Core.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Cart
{
    public class CartDetailRequest
    {
        public Guid ShopId { get; set; }
        public Guid CartId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
        public double Price { get; set; }
        public string? Note { get; set; }
    }
}
