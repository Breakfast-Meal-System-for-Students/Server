using AutoMapper;
using Azure.Core;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Models.Requests.Category;
using BMS.BLL.Models.Responses.Admin;
using BMS.BLL.Models.Responses.Cart;
using BMS.BLL.Models.Responses.Users;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Constants;
using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using BMS.Core.Exceptions;
using BMS.Core.Helpers;
using BMS.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services
{
    public class OrderService : BaseService, IOrderService
    {
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<ServiceActionResult> ChangeOrderStatus(Guid id, OrderStatus status)
        {
            var order = await _unitOfWork.OrderRepository.FindAsync(id);
            var canParsed = Enum.TryParse(order.Status, true, out OrderStatus s);
            if (canParsed)
            {
                if (s.Equals(8)) throw new BusinessRuleException("Order is Completed, so that you can not change this");
                if (s.Equals(7)) throw new BusinessRuleException("Order is Canceled, so that you can not change this");
                if(Enum.Equals(status, s))
                {
                    throw new BusinessRuleException($"Order is already in {status}, so that you can not change this");
                }
                if (s.CompareTo(status) > 0) throw new BusinessRuleException($"Order is already in {s}, so that you can not change back to {status}");
                order.Status = status.ToString();
                return new ServiceActionResult() { Detail = $" Change Order Status from {s} to {status} sucessfully" };
            } else
            {
                throw new BusinessRuleException("Have Error in Status Order");
            }
        }

        public async Task<ServiceActionResult> CreateOrder(Guid cartId, Guid voucherId)
        {
            var carts = (await _unitOfWork.CartRepository.GetAllAsyncAsQueryable()).Where(x => x.Id == cartId).Include(y => y.CartDetails).SingleOrDefault();
            var coupon = await _unitOfWork.CouponRepository.FindAsync(voucherId);
            if(carts == null) { return new ServiceActionResult() { Detail = $"Cart is not exits or deleted" }; }
            if (coupon == null) { return new ServiceActionResult() { Detail = $"Coupon is not exits or deleted" }; }
            Order order = new Order();
            order.Id = Guid.NewGuid();
            order.Status = OrderStatus.DRAFT.ToString();
            order.ShopId = carts.ShopId;
            order.CustomerId = carts.CustomerId;
            order.TotalPrice = carts.CartDetails.Sum(x => (x.Quantity * x.Price));
            await _unitOfWork.OrderRepository.AddAsync(order);
            foreach (var item in carts.CartDetails)
            {
                OrderItem orderItem = new OrderItem();
                orderItem.Id = Guid.NewGuid();
                orderItem.Id = item.Id;
                orderItem.OrderId = order.Id;
                orderItem.ProductId = item.ProductId;
                orderItem.Quantity = item.Quantity;
                orderItem.Price = item.Price;
                await _unitOfWork.OrderItemRepository.AddAsync(orderItem);
            }

            CouponUsage couponUsage = new CouponUsage();
            couponUsage.Id = Guid.NewGuid();
            couponUsage.CouponId = voucherId;
            couponUsage.OrderId = order.Id;
            couponUsage.UserId = order.CustomerId;
            await _unitOfWork.CouponUsageRepository.AddAsync(couponUsage);
            Notification notification = new Notification();
            notification.Id = Guid.NewGuid();
            notification.UserId = order.CustomerId;
            notification.OrderId = order.Id;
            notification.ShopId = order.ShopId;
            notification.Object = "Order";
            notification.Status = NotificationStatus.Draft.ToString();
            await _unitOfWork.NotificationRepository.AddAsync(notification);
            return new ServiceActionResult() { Detail = "Order is already create" };
        }

        public async Task<ServiceActionResult> GetListOrders(SearchOrderRequest request)
        {
            IQueryable<Order> orderQuery = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Include(a => a.OrderItems);

            //var canParsed = Enum.TryParse(request.Status, true, out OrderStatus status);
            if (request.Status != 0)
            {
                orderQuery = orderQuery.Where(m => m.Status.Equals(request.Status));
            }

            //if (!string.IsNullOrEmpty(request.Search))
            //{
            //    orderQuery = orderQuery.Where(m => m.Email.Contains(request.Search) || (m.LastName + m.FirstName).Contains(request.Search));
            //}

            orderQuery = request.IsDesc ? orderQuery.OrderByDescending(a => a.CreateDate) : orderQuery.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Order, OrderResponse>(_mapper, orderQuery, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };
        }

        public async Task<ServiceActionResult> GetOrderByID(Guid id)
        {
            var order = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Include(a => a.OrderItems).Where(x => x.Id == id).SingleOrDefault();
            if (order != null)
            {
                var returnOrder = _mapper.Map<OrderResponse>(order);

                return new ServiceActionResult(true) { Data = returnOrder };
            }
            else
            {
                return new ServiceActionResult(false, "Order is not exits or deleted");
            }
        }

        public async Task<ServiceActionResult> GetOrderByShop(Guid id, SearchOrderRequest request)
        {
            IQueryable<Order> orderQuery = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Include(a => a.OrderItems).Where(x => x.ShopId == id);

            //var canParsed = Enum.TryParse(request.Status, true, out OrderStatus status);
            if (request.Status != 0)
            {
                orderQuery = orderQuery.Where(m => m.Status.Equals(request.Status));
            }

            //if (!string.IsNullOrEmpty(request.Search))
            //{
            //    orderQuery = orderQuery.Where(m => m.Email.Contains(request.Search) || (m.LastName + m.FirstName).Contains(request.Search));
            //}

            orderQuery = request.IsDesc ? orderQuery.OrderByDescending(a => a.CreateDate) : orderQuery.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Order, OrderResponse>(_mapper, orderQuery, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };
        }

        public async Task<ServiceActionResult> GetOrderByUser(Guid id, SearchOrderRequest request)
        {
            IQueryable<Order> orderQuery = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Include(a => a.OrderItems).Where(x => x.CustomerId == id);

            //var canParsed = Enum.TryParse(request.Status, true, out OrderStatus status);
            if (request.Status != 0)
            {
                orderQuery = orderQuery.Where(m => m.Status.Equals(request.Status));
            }

            //if (!string.IsNullOrEmpty(request.Search))
            //{
            //    orderQuery = orderQuery.Where(m => m.Email.Contains(request.Search) || (m.LastName + m.FirstName).Contains(request.Search));
            //}

            orderQuery = request.IsDesc ? orderQuery.OrderByDescending(a => a.CreateDate) : orderQuery.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Order, OrderResponse>(_mapper, orderQuery, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };
        }

        public async Task<ServiceActionResult> GetStatusOrder(Guid orderId)
        {
            var order = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Where(x => x.Id == orderId).SingleOrDefault();
            if (order != null)
            {
                var returnOrder = _mapper.Map<OrderResponse>(order);

                return new ServiceActionResult(true) { Data = order.Status };
            }
            else
            {
                return new ServiceActionResult(false, "Order is not exits or deleted");
            }
        }

        public async Task<ServiceActionResult> GetTotalOrder(TotalOrdersRequest request)
        {
            var orders = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable());
            if(request.Year != 0)
            {
                if(request.Month != 0)
                {
                    if(request.Day != 0)
                    {
                        orders = orders.Where(x => x.CreateDate.Year == request.Year && x.CreateDate.Month == request.Month && x.CreateDate.Day == request.Day);
                    } else
                    {
                        orders = orders.Where(x => x.CreateDate.Year == request.Year && x.CreateDate.Month == request.Month);
                    }
                } else
                {
                    orders = orders.Where(x => x.CreateDate.Year == request.Year);
                }
            }
            if(request.Status != 0)
            {
                orders = orders.Where(x => x.Status.Equals(request.Status.ToString()));
            }
            return new ServiceActionResult()
            {
                Data = orders.Count()
            };
        }

        public async Task<ServiceActionResult> UpdateStatusOrder(Guid id, string status)
        {


            var order = await _unitOfWork.OrderRepository.FindAsync(id) ?? throw new ArgumentNullException("Order is not exist");
            order.Status = status;
     
            order.LastUpdateDate = DateTime.Now;
           
            await _unitOfWork.CommitAsync();

            return new ServiceActionResult(true) { Data = order };
        }

    }
}
