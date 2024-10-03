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
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }

        public Guid ShopId { get; set; }
        public bool IsGroup { get; set; }
        public ICollection<CartDetailResponse> CartDetails { get; set; } = new List<CartDetailResponse>();
        public ICollection<CartGroupUserResponse> CartGroupUserResponses { get; set; } = new List<CartGroupUserResponse>();
    }
}
