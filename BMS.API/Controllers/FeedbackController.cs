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
        [Authorize]
        public async Task<IActionResult> CreateFeedback(FeedbackRequest request)
        {
            return await ExecuteServiceLogic(
                               async () => await _feedbackService.AddFeedback(request).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpGet("{shopId}")]
        [Authorize]
        public async Task<IActionResult> GetFeedbacksOfAMentor(Guid shopId, [FromQuery] GetFeedbackInShop request)
        {
            return await ExecuteServiceLogic(
                               async () => await _feedbackService.GetAllFeedbacksOfAShop(shopId, request).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetFeedbacksForStaff([FromQuery] FeedbackForStaffRequest pagingRequest)
        {
            return await ExecuteServiceLogic(
                               async () => await _feedbackService.GetAllFeedbacksForStaff( pagingRequest).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpPut("{Id}")]
        [Authorize]
        public async Task<IActionResult> ReviewFeedback([FromBody] ReviewFeedbackRequest request)
        {
            return await ExecuteServiceLogic(
                               async () => await _feedbackService.ReviewFeedback(request.Id, request.Status).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpGet("CheckOrderIsFeedbacked")]
        [Authorize]
        public async Task<IActionResult> CheckOrderIsFeedbacked(Guid orderId)
        {
            return await ExecuteServiceLogic(
                               async () => await _feedbackService.CheckOrderIsFeedbacked(orderId).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
    }
}
