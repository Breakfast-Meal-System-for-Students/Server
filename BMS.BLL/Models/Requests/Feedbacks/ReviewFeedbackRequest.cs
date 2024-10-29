using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Feedbacks
{
    public class ReviewFeedbackRequest
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
    }
}
