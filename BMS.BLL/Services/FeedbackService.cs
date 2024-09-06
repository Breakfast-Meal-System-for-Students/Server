using AutoMapper;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Basic;
using BMS.BLL.Models.Requests.Feedbacks;
using BMS.BLL.Models.Responses.Feedbacks;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Entities;
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
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper)
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

            var feedbackQueryable = (await _unitOfWork.FeedbackRepository.GetAllAsyncAsQueryable());
               

            var paginatedFeedback = PaginationHelper.BuildPaginatedResult<Feedback, FeedbackResponse>(_mapper, feedbackQueryable, request.PageSize, request.PageIndex);

            return new ServiceActionResult() { Data = paginatedFeedback };
        }

    }
}
