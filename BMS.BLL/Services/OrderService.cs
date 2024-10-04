using AutoMapper;
using Azure;
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
        private readonly IQRCodeService _qrCodeService;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IQRCodeService qrCodeService) : base(unitOfWork, mapper)
        {
            _qrCodeService = qrCodeService;
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
                if (status.Equals(OrderStatus.COMPLETE))
                {
                    if (bool.Parse((await CheckOrderIsPayed(id)).Data.ToString()) == false)
                    {
                        Transaction transaction = new Transaction()
                        {
                            OrderId = order.Id,
                            Price = Convert.ToDouble(order.TotalPrice),
                            Method = TransactionMethod.Cash.ToString(),
                            Status = TransactionStatus.PAID
                        };

                        await _unitOfWork.TransactionRepository.AddAsync(transaction);
                    }
                }
                order.Status = status.ToString();
                order.LastUpdateDate = DateTime.Now;
                return new ServiceActionResult() { Detail = $" Change Order Status from {s} to {status} sucessfully" };
            } else
            {
                throw new BusinessRuleException("Have Error in Status Order");
            }
        }

        public async Task<ServiceActionResult> CheckOrderIsPayed(Guid orderId)
        {
            var transactions = (await _unitOfWork.TransactionRepository.GetAllAsyncAsQueryable())
                               .Where(x => x.OrderId == orderId && x.Status == TransactionStatus.PAID).SingleOrDefault();
            return new ServiceActionResult()
            {
                Data = transactions == null ? false : true,
            };
        }

        public async Task<ServiceActionResult> CreateOrder(Guid cartId, Guid voucherId)
        {
            var carts = (await _unitOfWork.CartRepository.GetAllAsyncAsQueryable())
                        .Where(x => x.Id == cartId)
                        .Include(y => y.CartDetails)
                        .SingleOrDefault();

            if (carts == null)
            {
                return new ServiceActionResult() { Detail = "Cart does not exist or is deleted" };
            }

            

            Order order = new Order
            {
                Status = OrderStatus.DRAFT.ToString(),
                ShopId = carts.ShopId,
                CustomerId = carts.CustomerId,
                TotalPrice = carts.CartDetails.Sum(x => x.Quantity * x.Price)
            };

            double discount = 0;
            if (voucherId != Guid.Empty)
            {
                var coupon = await _unitOfWork.CouponRepository.FindAsync(voucherId);
                if (coupon == null || coupon.IsDeleted)
                {
                    return new ServiceActionResult() { Detail = "Coupon does not exist or is deleted" };
                }
                if (coupon.StartDate >= order.CreateDate)
                {
                    return new ServiceActionResult(false) { Detail = "The Date of Coupon is not yet start" };
                } else if (coupon.EndDate <= order.CreateDate)
                {
                    return new ServiceActionResult(false) { Detail = "The Date of Coupon is Finish" };
                } else if (order.TotalPrice < coupon.MinPrice)
                {
                    return new ServiceActionResult(false) { Detail = "Total Price of Order is not enough to use this voucher" };
                }    
                if (coupon.isPercentDiscount)
                {
                    discount = (order.TotalPrice * coupon.PercentDiscount) > coupon.MaxDiscount ? coupon.MaxDiscount : (order.TotalPrice * coupon.PercentDiscount);
                } else
                {
                    discount = coupon.MinDiscount;
                }
            }
            if(discount >= 0)
            {
                order.TotalPrice -= discount;
            }

            string qrContent = order.Id.ToString();
            order.QRCode = _qrCodeService.GenerateQRCode(qrContent);
            while (await CheckQRCodeExist(order.QRCode))
            {
                order.QRCode = _qrCodeService.GenerateQRCode(qrContent);
            }
            await _unitOfWork.OrderRepository.AddAsync(order);

            foreach (var item in carts.CartDetails)
            {
                OrderItem orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                };

                await _unitOfWork.OrderItemRepository.AddAsync(orderItem);
            }

            if (voucherId != Guid.Empty)
            {
                CouponUsage couponUsage = new CouponUsage
                {
                    CouponId = voucherId,
                    OrderId = order.Id,
                    UserId = order.CustomerId
                };

                await _unitOfWork.CouponUsageRepository.AddAsync(couponUsage);
            }

            Notification notification = new Notification
            {
                UserId = order.CustomerId,
                OrderId = order.Id,
                ShopId = order.ShopId,
                Object = "Order",
                Status = NotificationStatus.UnRead
            };

            await _unitOfWork.NotificationRepository.AddAsync(notification);

            await _unitOfWork.CartRepository.DeleteAsync(cartId);
            return new ServiceActionResult() { Detail = "Order has been created successfully" };

        }

        public async Task<ServiceActionResult> GetListOrders(SearchOrderRequest request)
        {
            IQueryable<Order> orderQuery = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Include(a => a.OrderItems).Include(b => b.Customer);

            //var canParsed = Enum.TryParse(request.Status, true, out OrderStatus status);
            if (request.Status != 0)
            {
                orderQuery = orderQuery.Where(m => m.Status.Equals(request.Status.ToString()));
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
            var order = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Include(a => a.OrderItems).Include(b => b.Customer).Where(x => x.Id == id).SingleOrDefault();
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
            IQueryable<Order> orderQuery = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Include(a => a.OrderItems).Include(b => b.Customer).Where(x => x.ShopId == id);

            //var canParsed = Enum.TryParse(request.Status, true, out OrderStatus status);
            if (request.Status != 0)
            {
                orderQuery = orderQuery.Where(m => m.Status.Equals(request.Status.ToString()));
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
            IQueryable<Order> orderQuery = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Include(a => a.OrderItems).Include(b => b.Customer).Where(x => x.CustomerId == id);

            //var canParsed = Enum.TryParse(request.Status, true, out OrderStatus status);
            if (request.Status != 0)
            {
                orderQuery = orderQuery.Where(m => m.Status.Equals(request.Status.ToString()));
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

        public async Task<ServiceActionResult> GetOrderForUser(Guid userId, SearchOrderRequest request)
        {
            IQueryable<Order> orderQuery = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Include(a => a.OrderItems).Include(b => b.Customer).Where(x => x.CustomerId == userId);

            if (request.Status != 0)
            {
                orderQuery = orderQuery.Where(m => m.Status.Equals(request.Status.ToString()));
            }

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

        private string GetBase64QRCode(byte[] QRcode)
        {
            return Convert.ToBase64String(QRcode);
        }

        private async Task<bool> CheckQRCodeExist(byte[] QRcode)
        {
            return (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Where(x => x.QRCode.Equals(QRcode)).Count() != 0;
        }

        public async Task<ServiceActionResult> CheckQRCodeOfUser(byte[] QRcode, Guid userId)
        {
            var order = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Where(x => x.QRCode.Equals(QRcode) && x.CustomerId == userId).FirstOrDefault();
            if (order != null)
            {
                return new ServiceActionResult()
                {
                    Data = _mapper.Map<OrderResponse>(order)
                };
            } else
            {
                return new ServiceActionResult()
                {
                    Detail = "Order is not of User"
                };
            }
        }
    }
}
