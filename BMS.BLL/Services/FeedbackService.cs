using AutoMapper;
using BMS.BLL.Helpers;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Basic;
using BMS.BLL.Models.Requests.Feedbacks;
using BMS.BLL.Models.Responses.Feedbacks;
using BMS.BLL.Models.Responses.Shop;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.BLL.Utilities;
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

        public async Task<ServiceActionResult> AddFeedback(FeedbackRequest request)
        {
            var shop = await _unitOfWork.ShopRepository.FindAsync(request.ShopId);
            if (shop == null)
            {
                return new ServiceActionResult
                {
                    Detail = "Shop is not valid or delete"
                };
            }
            int count = (await _unitOfWork.FeedbackRepository.GetAllAsyncAsQueryable()).Where(x => x.ShopId == request.ShopId && x.Status == FeedbackStatus.APPROVED).Count();
            shop.Rate = RatingHelper.CaculateRaingForShopWhenHaveNewFeedback(shop.Rate.GetValueOrDefault(0.0), request.Rating, count);
            await _unitOfWork.ShopRepository.UpdateAsync(shop);
            var feedbackEntity = _mapper.Map<Feedback>(request);
            feedbackEntity.Status = FeedbackStatus.APPROVED;
            await _unitOfWork.FeedbackRepository.AddAsync(feedbackEntity);
            
            return new ServiceActionResult();
        }

        public async Task<ServiceActionResult> GetAllFeedbacksOfAShop(Guid shopId, GetFeedbackInShop request)
        {
            //  var shop = (await _unitOfWork.ShopRepository.FindAsync(shopId)) ?? throw new ArgumentNullException("Shop not found");

            var feedbackQueryable = (await _unitOfWork.FeedbackRepository.GetAllAsyncAsQueryable()).Include(a => a.User).Include(a => a.Shop).Where(a => a.ShopId==shopId);

            if (request.Rating != 0)
            {
                feedbackQueryable = feedbackQueryable.Where(m => m.Rate == request.Rating);
            }

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

        public async Task<ServiceActionResult> CheckOrderIsFeedbacked(Guid orderId)
        {
            var order = await _unitOfWork.OrderRepository.FindAsync(orderId);
            if (order.Status != OrderStatus.COMPLETE.ToString())
            {
                return new ServiceActionResult(false) 
                {
                    Data = false,
                    Detail = "Order is not Complete"
                };
            }
            var feedback = (await _unitOfWork.FeedbackRepository.GetAllAsyncAsQueryable()).Where(x => x.OrderId == orderId).Count();
            if (feedback > 0)
            {
                return new ServiceActionResult(true)
                {
                    Data = true,
                };
            } else
            {
                return new ServiceActionResult(true)
                {
                    Data = false,
                };
            }
        }
    }
}
