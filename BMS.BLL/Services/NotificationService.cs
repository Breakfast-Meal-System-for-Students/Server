﻿using AutoMapper;
using Azure.Core;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Notification;
using BMS.BLL.Models.Responses.Admin;
using BMS.BLL.Models.Responses.Notification;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using BMS.Core.Exceptions;
using BMS.Core.Exceptions.IExceptions;
using BMS.Core.Helpers;
using BMS.DAL;
using Microsoft.AspNetCore.SignalR;
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

        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<NotificationHub> hubContext) : base(unitOfWork, mapper)
        {
            _hubContext = hubContext;
        }

        public async Task<ServiceActionResult> ChangeStatusNotification(Guid userId, Guid notificationId)
        {
            var notification = await _unitOfWork.NotificationRepository.FindAsync(notificationId);
            if(notification == null) { throw new BusinessRuleException("Invalid Notification"); }
            if(notification.UserId != userId)
            {
                throw new BusinessRuleException("Invalid userId");
            }
            notification.Status = NotificationStatus.Readed;
            return new ServiceActionResult() 
            {
                Data = notification, 
                Detail = "Notification is Readed"
            };
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

        public async Task<ServiceActionResult> CreateNotification(Order order)
        {
            Notification notification = new Notification()
            {
                Object = "Đơn hàng sắp đến giờ nhận món. Gôm các món ăn sau: Hãy sắp xếp thời gian.",
                Title = NotificationTitle.BOOKING_ORDER,
                Status = NotificationStatus.UnRead,
                UserId = order.CustomerId,
                ShopId = order.ShopId,
                OrderId = order.Id,
            };
            foreach(OrderItem orderItem in order.OrderItems) 
            {
                notification.Object += "\n";
                notification.Object += $"{orderItem.Product.Name}";
                notification.Object += orderItem.Product.PrepareTime != 0 ? $": {orderItem.Product.PrepareTime}" : "";
            }
            await _unitOfWork.NotificationRepository.AddAsync(notification);
            await _hubContext.Clients.User(notification.UserId.ToString()).SendAsync($"Create Notification Automatically", notification.Object);
            return new ServiceActionResult() { Data = notification };
        }

        public async Task<List<Notification>> GetAllNotificationsToSendMail(NotificationStatus status)
        {
            return (await _unitOfWork.NotificationRepository.GetAllAsyncAsQueryable())
                    .Include(a => a.User)
                    .Include(b => b.Shop)
                    .Include(c => c.Order)
                    .Where(x => x.Status == status).ToList();
        }

        public async Task<List<Notification>> GetAllNotificationsToSendNoti(Order order)
        {
            return (await _unitOfWork.NotificationRepository.GetAllAsyncAsQueryable())
                   .Include(a => a.User)
                   .Include(b => b.Shop)
                   .Include(c => c.Order)
                   .Where(x => x.Order.Id == order.Id && x.Status == NotificationStatus.UnRead).ToList();
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
