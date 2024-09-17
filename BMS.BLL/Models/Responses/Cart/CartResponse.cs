using BMS.Core.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.Cart
{
    public class CartResponse
    {
        public Guid CustomerId { get; set; }


        public bool IsPurchase { get; set; }
        public ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();
    }
}
