using AutoMapper;
using BMS.BLL.Helpers;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Basic;
using BMS.BLL.Models.Requests.Feedbacks;
using BMS.BLL.Models.Responses.Feedbacks;
using BMS.BLL.Models.Responses.Shop;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using BMS.Core.Helpers;
using BMS.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services
{
    public class FeedbackService :BaseService, IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceActionResult> AddFeedback(FeedbackRequest request, Guid userId)
        {
       

            var feedbackEntity = _mapper.Map<Feedback>(request);

            // do something add feedback
            await _unitOfWork.CommitAsync();

            return new ServiceActionResult();
        }

        public async Task<ServiceActionResult> GetAllFeedbacksOfAShop(Guid shopId, PagingRequest request)
        {
            //  var shop = (await _unitOfWork.ShopRepository.FindAsync(shopId)) ?? throw new ArgumentNullException("Shop not found");

            var feedbackQueryable = (await _unitOfWork.FeedbackRepository.GetAllAsyncAsQueryable()).Include(a => a.User).Include(a => a.Shop).Where(a => a.ShopId==shopId);
               

            var paginatedFeedback = PaginationHelper.BuildPaginatedResult<Feedback, FeedbackForStaffResponse>(_mapper, feedbackQueryable, request.PageSize, request.PageIndex);

            return new ServiceActionResult() { Data = paginatedFeedback };
        }

        public async Task<ServiceActionResult> GetAllFeedbacksForStaff(FeedbackForStaffRequest queryParameters)
        {
           
            //var feedbackQueryable1 = (await _unitOfWork.FeedbackRepository.GetAllAsyncAsQueryable()).Include(a => a.User).Include(a=> a.Shop);
            IQueryable<Feedback> feedbackQueryable = (await _unitOfWork.FeedbackRepository.GetAllAsyncAsQueryable()).Include(a => a.User).Include(a=> a.Shop);

            var canParsed = Enum.TryParse(queryParameters.Status, true, out FeedbackStatus status);
            if (canParsed)
            {
                feedbackQueryable = feedbackQueryable.Where(m => m.Status== status);
            }

            if (!string.IsNullOrEmpty(queryParameters.Search))
            {
                feedbackQueryable = feedbackQueryable.Where(m => m.Description.Contains(queryParameters.Search));
            }

            if (queryParameters.Rate != 0)
            {
                feedbackQueryable = feedbackQueryable.Where(m => m.Rate == queryParameters.Rate);
            }

            feedbackQueryable = queryParameters.IsDesc ? feedbackQueryable.OrderByDescending(a => a.CreateDate) : feedbackQueryable.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Feedback, FeedbackForStaffResponse>(_mapper, feedbackQueryable, queryParameters.PageSize, queryParameters.PageIndex);



            return new ServiceActionResult() { Data = paginationResult };
        }

        public async Task<ServiceActionResult> ReviewFeedback(Guid id, string status)
        {
            var feedback = await _unitOfWork.FeedbackRepository.FindAsync(id) ?? throw new ArgumentNullException("Feedback is not exist");
            feedback.Status = FeedbackStatus.BAN;
            await _unitOfWork.CommitAsync();
            return new ServiceActionResult();
        }

    }
}
