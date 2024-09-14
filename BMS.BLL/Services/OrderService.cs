using AutoMapper;
using Azure.Core;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Models.Responses.Admin;
using BMS.BLL.Models.Responses.Users;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Constants;
using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using BMS.Core.Exceptions;
using BMS.Core.Helpers;
using BMS.DAL;
using BMS.DAL.Migrations;
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
            var order = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Include(a => a.OrderItems).Where(x => x.Id == id);
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
            return new ServiceActionResult()
            {
                Data = orders.Count()
            };
        }
    }
}
