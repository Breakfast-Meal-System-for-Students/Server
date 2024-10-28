using BMS.BLL.Models.Responses.Admin;
using BMS.BLL.Models.Responses.Feedbacks;
using BMS.Core.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.Users
{
    public class GetOrdersAndFeedbackOfUserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Avatar { get; set; }
        public string Phone { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public ICollection<OrderResponse>? Orders { get; set; } = new List<OrderResponse>();

        public ICollection<FeedbackResponse>? Feedbacks { get; set; } = new List<FeedbackResponse>();
        public ICollection<CouponUsage>? CouponUsages { get; set; } = new List<CouponUsage>();
    }
}
