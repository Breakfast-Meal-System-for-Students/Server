using BMS.Core.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.Core.Domains.Entities;
using BMS.BLL.Models.Responses.Image;
namespace BMS.BLL.Models.Responses.Admin
{
    public class OrderItemResponse
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public ICollection<ImageResponse> ProductImages { get; set; }
        public Guid OrderId { get; set; }
        public Guid? UserId { get; set; }
    }
}
