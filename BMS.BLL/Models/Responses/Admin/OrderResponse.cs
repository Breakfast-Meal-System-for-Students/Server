using BMS.Core.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.Admin
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public double TotalPrice { get; set; }
        public string Status { get; set; } = null!;

        public Guid CustomerId { get; set; }
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
        public string? Avatar { get; set; }
        public byte[] QRCode { get; set; }
        public Guid ShopId { get; set; }
        public Core.Domains.Entities.Shop Shop { get; set; } = null!;

        public ICollection<OrderItemResponse> OrderItems { get; set; } = new List<OrderItemResponse>();
    }
}
