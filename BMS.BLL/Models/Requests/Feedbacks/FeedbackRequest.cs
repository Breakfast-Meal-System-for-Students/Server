using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Feedbacks
{
    public class FeedbackRequest
    {
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }
        public string Description { get; set; } = null!;
        //public FeedbackStatus Status { get; set; }
        public Guid UserId { get; set; }
        public Guid ShopId { get; set; }
        public Guid OrderId { get; set; }

    }
}
