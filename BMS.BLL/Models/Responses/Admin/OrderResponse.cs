using BMS.Core.Domains.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        public DateTime? OrderDate { get; set; }
        public Guid CustomerId { get; set; }
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
        public string? Avatar { get; set; }
        public string QRCode { get; set; }
        public Guid ShopId { get; set; }
        public string? ShopName { get; set; }
        public string? ShopImage { get; set; } = null!;
        public ICollection<OrderItemResponse> OrderItems { get; set; } = new List<OrderItemResponse>();
        public bool IsGroup { get; set; }
        public bool canCancel {  get; set; }
        public bool canFeedback { get; set; }
        public bool isPayed { get; set; }
    }
}
