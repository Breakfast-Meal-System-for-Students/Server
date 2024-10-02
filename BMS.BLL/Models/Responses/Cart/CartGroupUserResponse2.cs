using BMS.Core.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.Cart
{
    public class CartGroupUserResponse2
    {
        public Guid CartId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<CartDetailResponse> CartDetails { get; set; } = new List<CartDetailResponse>();
    }
}
