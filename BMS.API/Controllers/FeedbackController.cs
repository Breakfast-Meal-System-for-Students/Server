using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.Basic;
using BMS.BLL.Models.Requests.Feedbacks;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
    public class FeedbackController : BaseApiController
    {
        private readonly IFeedbackService _feedbackService;
        //private readonly UserClaims _userClaims;
        //private readonly IUserClaimsService _userClaimsService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
            // _userClaimsService = userClaimsService;
            // _userClaims = _userClaimsService.GetUserClaims();
            _baseService = (BaseService)_feedbackService;
        }

    
        [HttpPost("send-feedback")]
        
        public async Task<IActionResult> CreateFeedback(FeedbackRequest request)
        {
            Guid guid = Guid.NewGuid();
            //demo thoi nhoe
            return await ExecuteServiceLogic(
                               async () => await _feedbackService.AddFeedback(request, guid).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpGet("{shopId}")]
        public async Task<IActionResult> GetFeedbacksOfAMentor(Guid shopId, [FromQuery] PagingRequest pagingRequest)
        {
            return await ExecuteServiceLogic(
                               async () => await _feedbackService.GetAllFeedbacksOfAShop(shopId, pagingRequest).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpGet]
        public async Task<IActionResult> GetFeedbacksForStaff([FromQuery] FeedbackForStaffRequest pagingRequest)
        {
            return await ExecuteServiceLogic(
                               async () => await _feedbackService.GetAllFeedbacksForStaff( pagingRequest).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> ReviewFeedback(Guid Id, string status)
        {
            return await ExecuteServiceLogic(
                               async () => await _feedbackService.ReviewFeedback(Id, status).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
    }
}
