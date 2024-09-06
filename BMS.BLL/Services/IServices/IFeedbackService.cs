using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Basic;
using BMS.BLL.Models.Requests.Feedbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.IServices
{
    public interface IFeedbackService
    {
        Task<ServiceActionResult> GetAllFeedbacksOfAShop(Guid shopId, PagingRequest request);
        Task<ServiceActionResult> AddFeedback(FeedbackRequest  request, Guid userId);
     

    }
}
