using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Basic;
using BMS.BLL.Models.Requests.Feedbacks;
using BMS.BLL.Models.Responses.Feedbacks;
using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using BMS.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.IServices
{
    public interface IFeedbackService
    {
        Task<ServiceActionResult> GetAllFeedbacksOfAShop(Guid shopId, GetFeedbackInShop request);
        Task<ServiceActionResult> AddFeedback(FeedbackRequest  request);

        Task<ServiceActionResult> GetAllFeedbacksForStaff(FeedbackForStaffRequest queryParameters);
        Task<ServiceActionResult> ReviewFeedback(Guid id, string status);
        Task<ServiceActionResult> CheckOrderIsFeedbacked(Guid orderId);
    }
}
