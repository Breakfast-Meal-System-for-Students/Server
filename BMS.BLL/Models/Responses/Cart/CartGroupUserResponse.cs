using BMS.Core.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.Cart
{
    public class CartGroupUserResponse
    {
        public Guid CartId { get; set; }
        public Guid UserId { get; set; }
        //public string? firstname { get; set; } = null!;
        //public string? lastname { get; set; } = null!;
        //public string? avatar { get; set; }
        //public ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();
    }
}
