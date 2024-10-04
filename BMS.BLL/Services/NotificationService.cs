using AutoMapper;
using Azure.Core;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Notification;
using BMS.BLL.Models.Responses.Admin;
using BMS.BLL.Models.Responses.Notification;
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
    public class NotificationService : BaseService, INotificationService
    {
        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        public async Task<ServiceActionResult> CountNotificationForShop(Guid shopId)
        {
            IQueryable<Notification> notificationQuery = (await _unitOfWork.NotificationRepository.GetAllAsyncAsQueryable()).Where(x => x.ShopId == shopId && x.Status == NotificationStatus.UnRead);

            return new ServiceActionResult(true) { Data = notificationQuery.Count() };
        }

        public async Task<ServiceActionResult> CountNotificationForUser(Guid userId)
        {
            IQueryable<Notification> notificationQuery = (await _unitOfWork.NotificationRepository.GetAllAsyncAsQueryable()).Where(x => x.UserId == userId && x.Status == NotificationStatus.UnRead);

            return new ServiceActionResult(true) { Data = notificationQuery.Count() };
        }

        public async Task<List<Notification>> GetAllNotificationsToSendMail(NotificationStatus status)
        {
            return (await _unitOfWork.NotificationRepository.GetAllAsyncAsQueryable())
                    .Include(a => a.User)
                    .Include(b => b.Shop)
                    .Include(c => c.Order)
                    .Where(x => x.Status == status).ToList();
        }

        public async Task<ServiceActionResult> GetNotificationForShop(Guid shopId, GetNotificationRequest request)
        {
            IQueryable<Notification> notificationQuery = (await _unitOfWork.NotificationRepository.GetAllAsyncAsQueryable()).Where(x => x.ShopId == shopId);

            if (request.Status != 0)
            {
                notificationQuery = notificationQuery.Where(m => m.Status == request.Status);
            }

            if (request.Title != 0)
            {
                notificationQuery = notificationQuery.Where(m => m.Title == request.Title);
            }
            notificationQuery = notificationQuery.OrderByDescending(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Notification, NotificationResponse>(_mapper, notificationQuery, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };
        }

        public async Task<ServiceActionResult> GetNotificationForUser(Guid userId, GetNotificationRequest request)
        {
            IQueryable<Notification> notificationQuery = (await _unitOfWork.NotificationRepository.GetAllAsyncAsQueryable()).Where(x => x.UserId == userId);

            if (request.Status != 0)
            {
                notificationQuery = notificationQuery.Where(m => m.Status == request.Status);
            }

            if (request.Title != 0)
            {
                notificationQuery = notificationQuery.Where(m => m.Title == request.Title);
            }
            notificationQuery = notificationQuery.OrderByDescending(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Notification, NotificationResponse>(_mapper, notificationQuery, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };
        }

        public async Task SaveChange()
        {
            await _unitOfWork.CommitAsync();
        }
    }
}
