using BMS.BLL.Models.Responses.Image;
using BMS.Core.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.Cart
{
    public class CartDetailResponse
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }

        public Guid ProductId { get; set; }
        public ICollection<ImageResponse> Images { get; set; } = new List<ImageResponse>();
        public string Name {  get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string? Note { get; set; }
        public Guid? CartGroupUserId { get; set; }
        //public string CartGroupUserName { get; set; }
        //public string CartGroupUserImage { get; set; }
    }
}
