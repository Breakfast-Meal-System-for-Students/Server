using AutoMapper;
using Azure;
using Azure.Core;
using BMS.BLL.AI;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Models.Requests.Category;
using BMS.BLL.Models.Requests.Order;
using BMS.BLL.Models.Responses.Admin;
using BMS.BLL.Models.Responses.Cart;
using BMS.BLL.Models.Responses.Users;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.BLL.Utilities;
using BMS.Core.Domains.Constants;
using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using BMS.Core.Exceptions;
using BMS.Core.Helpers;
using BMS.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace BMS.BLL.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IQRCodeService _qrCodeService;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly RecommendationEngine _recommendationEngine;
        private readonly IProductService _productService;
        private readonly IAuthService _authService;
        private readonly IOpeningHoursService _openingHoursService;
        private readonly IWalletService _walletService;
        public OrderService(IUnitOfWork unitOfWork,IOpeningHoursService openingHoursService, IMapper mapper, IQRCodeService qrCodeService, IHubContext<NotificationHub> hubContext, RecommendationEngine recommendationEngine, IProductService productService, IAuthService authService, IWalletService walletService) : base(unitOfWork, mapper)
        {
            _qrCodeService = qrCodeService;
            _hubContext = hubContext;
            _recommendationEngine = recommendationEngine;
            _productService = productService;
            _authService = authService;
            _openingHoursService = openingHoursService;
            _walletService = walletService;
        }

        public async Task<ServiceActionResult> ChangeOrderStatus(Guid id, OrderStatus status, Guid userId)
        {
            var order = await _unitOfWork.OrderRepository.FindAsync(id);
            if(order == null)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Order is not Exist"
                };
            }
            List<string> rolesOfUser = (await _authService.GetRole(userId)).ToList();
            if (rolesOfUser.Contains(UserRoleConstants.USER))
            {
                if (order.CustomerId != userId)
                {
                    return new ServiceActionResult(false)
                    {
                        Detail = "Order is not of User"
                    };
                }
            }
            var canParsed = Enum.TryParse(order.Status, true, out OrderStatus s);
            if (canParsed)
            {
                if (s.Equals(OrderStatus.COMPLETE))
                {
                    return new ServiceActionResult(false)
                    {
                        Detail = "Order is Completed, so that you can not change this"
                    };
                }
                if (s.Equals(OrderStatus.CANCEL))
                {
                    return new ServiceActionResult(false)
                    {
                        Detail = "Order is Canceled, so that you can not change this"
                    };
                }
                if (Enum.Equals(status, s))
                {
                    return new ServiceActionResult(false)
                    {
                        Detail = $"Order is already in {status}, so that you can not change this"
                    };
                }
                if (s.CompareTo(status) > 0)
                {
                    return new ServiceActionResult(false)
                    {
                        Detail = $"Order is already in {s}, so that you can not change back to {status}"
                    };
                }
                if (order.OrderDate > DateTimeHelper.GetCurrentTime().AddHours(2) && status > OrderStatus.CHECKING && status != OrderStatus.CANCEL)
                {
                    return new ServiceActionResult(false)
                    {
                        Detail = $"Since the order time of this order is {order.OrderDate}, you cannot change the order to status:  {status}. Please wait until 2 hours before order time to prepare this order for the best taste."
                    };
                }
                if (status == OrderStatus.TAKENOVER && s != OrderStatus.PREPARED)
                {
                    return new ServiceActionResult(false)
                    {
                        Detail = $"Order is already not in PREPARED, so that you can not change to {status}"
                    };
                }
                //bool isPayed = bool.Parse((await CheckOrderIsPayed(id)).Data.ToString());
                Wallet wallet = null;
                bool isPayed = bool.Parse((await CheckOrderIsPayed(id)).Data.ToString());
                if (status.Equals(OrderStatus.CANCEL))
                {
                    if (!(s <= OrderStatus.CHECKING))
                    {
                        return new ServiceActionResult(false)
                        {
                            Detail = $"Order is already in {s}. So that not Cancel"
                        };
                    } else if (isPayed)
                    {
                        var x = await _walletService.UpdateBalanceInSystem(order.CustomerId, TransactionStatus.REFUND, ((decimal)order.TotalPrice), order.Id);
                        if(x < 0)
                        {
                            return new ServiceActionResult(false)
                            {
                                Detail = "The Wallet has been deleted or not exists. So The system can not refund."
                            };
                        }
                        var y = await _walletService.UpdateBalanceAdmin(TransactionStatus.REFUND, ((decimal)order.TotalPrice));
                        if (y < 0)
                        {
                            return new ServiceActionResult(false)
                            {
                                Detail = "The System Wallet has been deleted or not exists. So The system can not refund."
                            };
                        }
                    }
                }

                if (status.Equals(OrderStatus.COMPLETE))
                {
                    if (isPayed == false)
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
                    else
                    {
                        var shop = (await _unitOfWork.ShopRepository.GetAllAsyncAsQueryable()).Where(x => x.Id == order.ShopId).FirstOrDefault();
                        await _walletService.UpdateBalanceInSystem(shop.UserId.GetValueOrDefault(), TransactionStatus.PAIDTOSHOP, (decimal)order.TotalPrice, null);
                        await _walletService.UpdateBalanceAdmin(TransactionStatus.PAIDTOSHOP, (decimal)order.TotalPrice);
                        await _walletService.SaveChange();
                    }
                }
                order.Status = status.ToString();
                order.LastUpdateDate = DateTimeHelper.GetCurrentTime();
                await _unitOfWork.OrderRepository.UpdateAsync(order);
                await _unitOfWork.CommitAsync();
                List<Notification> notifications = new List<Notification>();
                if (!status.Equals(OrderStatus.TAKENOVER))
                {
                    Notification notification = new Notification
                    {
                        UserId = order.CustomerId,
                        OrderId = order.Id,
                        ShopId = order.ShopId,
                        Object = $"Order Update: The status of your order has been changed from {s} to {status}",
                        Status = NotificationStatus.UnRead,
                        Title = GetTitleNotification(status),
                        Destination = NotificationDestination.FORUSER
                    };
                    notifications.Add(notification);
                }

                if (status.Equals(OrderStatus.TAKENOVER) || status.Equals(OrderStatus.COMPLETE) || status.Equals(OrderStatus.CANCEL)))
                {
                    Notification notification1 = new Notification
                    {
                        UserId = order.CustomerId,
                        OrderId = order.Id,
                        ShopId = order.ShopId,
                        Object = $"Order Update: The status of your order has been changed from {s} to {status}",
                        Status = NotificationStatus.UnRead,
                        Title = GetTitleNotification(status),
                        Destination = NotificationDestination.FORSHOP
                    };
                    notifications.Add(notification1);
                }
                

                if (status.Equals(OrderStatus.COMPLETE))
                {
                    Notification notificatio2 = new Notification
                    {
                        UserId = order.CustomerId,
                        OrderId = order.Id,
                        ShopId = order.ShopId,
                        Object = $"Order Update: The status of your order has been changed from {s} to {status}",
                        Status = NotificationStatus.UnRead,
                        Title = GetTitleNotification(status),
                        Destination = NotificationDestination.FORSTAFF
                    };
                    notifications.Add(notificatio2);
                }
                await _unitOfWork.NotificationRepository.AddRangeAsync(notifications);
                //await _hubContext.Clients.User(order.CustomerId.ToString()).SendAsync("ReceiveNotification", notification.Object);
                //await _hubContext.Clients.User(order.ShopId.ToString()).SendAsync("ReceiveNotification", notification.Object);
                await _unitOfWork.CommitAsync();
                return new ServiceActionResult() { Detail = $" Change Order Status from {s} to {status} sucessfully" };
            } else
            {
                throw new BusinessRuleException("Have Error in Status Order");
            }
        }

        public async Task<ServiceActionResult> CheckOrderIsPayed(Guid orderId)
        {
            var transactions = (await _unitOfWork.TransactionRepository.GetAllAsyncAsQueryable())
                               .Where(x => x.OrderId == orderId && x.Status == TransactionStatus.PAID).FirstOrDefault();
            return new ServiceActionResult()
            {
                Data = transactions == null ? false : true,
            };
        }

        /*public async Task<ServiceActionResult> CreateOrder(CreateOrderRequest request)
        {
            var carts = (await _unitOfWork.CartRepository.GetAllAsyncAsQueryable())
                        .Where(x => x.Id == request.CartId)
                        .Include(y => y.CartDetails)
                        .SingleOrDefault();

            if (carts == null)
            {
                return new ServiceActionResult() { Detail = "Cart does not exist or is deleted" };
            }

            

            Order order = new Order
            {
                Status = OrderStatus.ORDERED.ToString(),
                ShopId = carts.ShopId,
                CustomerId = carts.CustomerId,
                TotalPrice = carts.CartDetails.Sum(x => x.Quantity * x.Price),
                OrderDate = request.OrderDate
            };

            double discount = 0;
            if (request.VoucherId != Guid.Empty && request.VoucherId != null)
            {
                var coupon = await _unitOfWork.CouponRepository.FindAsync(request.VoucherId);
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
                if(order.TotalPrice - discount < 0)
                {
                    return new ServiceActionResult(false) { Detail = "Total Price of Order is < 0" };
                }
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

            if (request.VoucherId != Guid.Empty)
            {
                CouponUsage couponUsage = new CouponUsage
                {
                    CouponId = request.VoucherId,
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
                Object = "Create Order",
                Status = NotificationStatus.UnRead,
                Title = NotificationTitle.BOOKING_ORDER
            };

            await _unitOfWork.NotificationRepository.AddAsync(notification);

            await _unitOfWork.CartRepository.DeleteAsync(request.CartId);
            await _hubContext.Clients.User(notification.UserId.ToString()).SendAsync("Create Order", notification.Object);
            return new ServiceActionResult() { Detail = "Order has been created successfully" };

        }*/

        public async Task<ServiceActionResult> CreateOrder(CreateOrderRequest request)
        {
            Expression<Func<Cart, bool>> filter = cart => (cart.Id == request.CartId);
            var cart = (await _unitOfWork.CartRepository.FindAsyncAsQueryable(filter))
                            .Include(y => y.CartDetails).ThenInclude(x => x.Product)
                            .FirstOrDefault();

            if (cart == null)
            {
                return new ServiceActionResult(false) { Detail = "Cart does not exist or is deleted" };
            }
            if (cart.CartDetails == null || !cart.CartDetails.Any())
            {
                return new ServiceActionResult(false) { Detail = "Cart is Empty. Please choose product and Add to Cart" };
            } 
            else
            {
                List<CartDetail> cartDetails = new List<CartDetail>();
                foreach (var item in cart.CartDetails)
                {
                    if (item.Product.isOutOfStock)
                    {
                        cartDetails.Add(item);
                    }
                }
                if (cartDetails.Any())
                {
                    await _unitOfWork.CartDetailRepository.DeleteRangeAsync(cartDetails);
                    await _unitOfWork.CommitAsync();
                    return new ServiceActionResult(false)
                    {
                        Detail = $"Some Products is out Of Stock. They are removed from your cart. Plese order again"
                    };
                }
            }
            /*else
            {
                var serviceActionResult = new ServiceActionResult(false) { Detail = String.Empty };
                var listCartDetail = cart.CartDetails.GroupBy(a => a.ProductId).Select(group => new
                {
                    Id = group.Key,
                    Name = group.FirstOrDefault().Product.Name,
                    Quantity = group.Sum(x => x.Quantity)
                });
                foreach (var item in listCartDetail)
                {
                    int inventory = await _productService.GetInventoryOfProductInDay(item.Id, request.OrderDate.GetValueOrDefault());
                    if (item.Quantity > inventory)
                    {
                        serviceActionResult.Detail += $"The Inventory Of {item.Name} In Shop is had already {inventory} now. Please booking this product less than or equals {inventory} {Environment.NewLine} ";
                    }
                }
                if (serviceActionResult.Detail != String.Empty)
                {
                    return serviceActionResult;
                }
            }*/
            // Check if the order date is within the shop's opening hours
            var requestOrderTimeConvertVN = DateTimeHelper.GetVietNameseTime(request.OrderDate.GetValueOrDefault());
            if (request.OrderDate != null)
            {
                bool isWithinOpeningHours = await _openingHoursService.IsWithinOpeningHours(cart.ShopId, requestOrderTimeConvertVN);
                if (!isWithinOpeningHours)
                {
                    // Get the current day based on the Vietnamese timezone
                    WeekDay currentDay = DateTimeHelper.GetWeekDayFromDateTime(requestOrderTimeConvertVN);

                    // Fetch opening hours for the shop and the current day
                    var openingHours = await _unitOfWork.OpeningHoursRepository
                        .FindAsync(x => x.ShopId == cart.ShopId && x.day == currentDay);

                    // Calculate the start and end times for the shop's opening hours
                    DateTime startTime = requestOrderTimeConvertVN.Date.AddHours(openingHours.from_hour)
                                                .AddMinutes(openingHours.from_minute);
                    DateTime endTime = requestOrderTimeConvertVN.Date.AddHours(openingHours.to_hour)
                                              .AddMinutes(openingHours.to_minute);


                    // Return a failure response if the order date is outside the shop's opening hours
                    return new ServiceActionResult(false)
                    {
                        IsSuccess = false,
                        Detail = "On " + currentDay + ", the shop will operate from " + startTime + " to " + endTime
                    };
                }
            }
            else
            {
                var requestOrderTimeDefaut = DateTimeHelper.GetCurrentTime();

                bool isWithinOpeningHours = await _openingHoursService.IsWithinOpeningHours(cart.ShopId, requestOrderTimeDefaut);
                if (!isWithinOpeningHours)
                {
                    // Get the current day based on the Vietnamese timezone
                    WeekDay currentDay = DateTimeHelper.GetWeekDayFromDateTime(requestOrderTimeDefaut);

                    // Fetch opening hours for the shop and the current day
                    var openingHours = await _unitOfWork.OpeningHoursRepository
                        .FindAsync(x => x.ShopId == cart.ShopId && x.day == currentDay);

                    // Calculate the start and end times for the shop's opening hours
                    DateTime startTime = requestOrderTimeDefaut.Date.AddHours(openingHours.from_hour)
                                                .AddMinutes(openingHours.from_minute);
                    DateTime endTime = requestOrderTimeDefaut.Date.AddHours(openingHours.to_hour)
                                              .AddMinutes(openingHours.to_minute);


                    // Return a failure response if the order date is outside the shop's opening hours
                    return new ServiceActionResult(false)
                    {
                        IsSuccess = false,
                        Detail = "On " + currentDay + ", the shop will operate from " + startTime + " to " + endTime
                    };
                }
            }
            Order order = new Order
            {
                Status = OrderStatus.ORDERED.ToString(),
                ShopId = cart.ShopId,
                CustomerId = cart.CustomerId,
                TotalPrice = cart.CartDetails.Sum(x => x.Quantity * x.Price),
                OrderDate = request.OrderDate == null ? null : DateTimeHelper.GetVietNameseTime(request.OrderDate.GetValueOrDefault())
            };

            double discount = 0;

            if (request.VoucherId != Guid.Empty && request.VoucherId != null)
            {
                var coupon = await _unitOfWork.CouponRepository.FindAsync(request.VoucherId);
                if (coupon == null || coupon.IsDeleted)
                {
                    return new ServiceActionResult(false) { Detail = "Coupon does not exist or is deleted" };
                }
                if (coupon.ShopId != order.ShopId)
                {
                    return new ServiceActionResult(false) { Detail = "Coupon is not used in this shop" };
                }
                if (coupon.StartDate >= order.CreateDate)
                {
                    return new ServiceActionResult(false) { Detail = $"The Date of Coupon is not yet start.The voucher can be used from {coupon.StartDate}" };
                }
                else if (coupon.EndDate <= order.CreateDate)
                {
                    return new ServiceActionResult(false) { Detail = $"The Date of Coupon is Finish on {coupon.EndDate}" };
                }
                else if (order.TotalPrice < coupon.MinDiscount)
                {
                    return new ServiceActionResult(false) { Detail = $"Total Price of Order is {order.TotalPrice} is must be more than or equal to {coupon.MinDiscount}" };
                }

                discount = coupon.isPercentDiscount
                    ? Math.Min(order.TotalPrice * coupon.PercentDiscount / 100, coupon.MaxDiscount)
                    : coupon.MinPrice;
            }

            if (discount > 0)
            {
                if (order.TotalPrice - discount <= 0)
                {
                    return new ServiceActionResult(false) { Detail = "Total Price of Order is <= 0" };
                }
                order.TotalPrice -= discount;
            }

            string qrContent = DateTimeHelper.GetCurrentTime().Ticks.ToString();
            order.QRCode = _qrCodeService.GenerateQRCode(qrContent);

            while (await CheckQRCodeExist(order.QRCode))
            {
                order.QRCode = _qrCodeService.GenerateQRCode(qrContent);
            }

            if (cart.IsGroup)
            {
                order.IsGroup = true;
            }
            await _unitOfWork.OrderRepository.AddAsync(order);

            var orderItems = cart.CartDetails.Select(item => new OrderItem
            {
                OrderId = order.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Price,
                Note = item.Note,
                UserId = item.CartGroupUserId
            }).ToList();

            await _unitOfWork.OrderItemRepository.AddRangeAsync(orderItems);

            if (request.VoucherId != Guid.Empty)
            {
                CouponUsage couponUsage = new CouponUsage
                {
                    CouponId = request.VoucherId,
                    OrderId = order.Id,
                    UserId = order.CustomerId
                };
                await _unitOfWork.CouponUsageRepository.AddAsync(couponUsage);
            }
            
            await _unitOfWork.CartDetailRepository.DeleteRangeAsync(cart.CartDetails);
            var cartGroupUsers = (await _unitOfWork.CartGroupUserRepository.GetAllAsyncAsQueryable()).Where(x => x.CartId == cart.Id).AsEnumerable();
            await _unitOfWork.CartGroupUserRepository.DeleteRangeAsync(cartGroupUsers);
            await _unitOfWork.CartRepository.DeleteAsync(cart.Id);

            await _unitOfWork.CommitAsync();
            List<Notification> notifications = new List<Notification>();
            Notification notification = new Notification
            {
                UserId = order.CustomerId,
                OrderId = order.Id,
                ShopId = order.ShopId,
                Object = "New Order Created! A customer has placed an order.",
                Status = NotificationStatus.UnRead,
                Title = NotificationTitle.BOOKING_ORDER,
                Destination = NotificationDestination.FORUSER
            };
            notifications.Add(notification);
            Notification notification1 = new Notification
            {
                UserId = order.CustomerId,
                OrderId = order.Id,
                ShopId = order.ShopId,
                Object = "New Order Created! A customer has placed an order.",
                Status = NotificationStatus.UnRead,
                Title = NotificationTitle.BOOKING_ORDER,
                Destination = NotificationDestination.FORSHOP
            };
            notifications.Add(notification1);
            Notification notification2 = new Notification
            {
                UserId = order.CustomerId,
                OrderId = order.Id,
                ShopId = order.ShopId,
                Object = "New Order Created! A customer has placed an order.",
                Status = NotificationStatus.UnRead,
                Title = NotificationTitle.BOOKING_ORDER,
                Destination = NotificationDestination.FORSTAFF
            };
            notifications.Add(notification2);
            await _unitOfWork.NotificationRepository.AddRangeAsync(notifications);
            await _unitOfWork.CommitAsync();
            /*await _hubContext.Clients.User(notification.UserId.ToString()).SendAsync("ReceiveNotification", notification.Object);
            await _hubContext.Clients.User(notification.ShopId.ToString()).SendAsync("ReceiveNotification", notification.Object);*/
            return new ServiceActionResult()
            {
                Data = order.Id,
                Detail = "Order has been created successfully" 
            };
        }

        public async Task<ServiceActionResult> GetListOrders(SearchOrderRequest request)
        {
            IQueryable<Order> orderQuery = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Include(a => a.OrderItems).Include(b => b.Customer).Include(c => c.Shop);

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
            var order = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Include(a => a.OrderItems).ThenInclude(d => d.Product).ThenInclude(e => e.Images).Include(b => b.Customer).Include(c => c.Shop).Where(x => x.Id == id).SingleOrDefault();
            if (order != null)
            {
                var returnOrder = _mapper.Map<OrderResponse>(order);

                if ((returnOrder.Status == OrderStatus.ORDERED.ToString() || returnOrder.Status == OrderStatus.CHECKING.ToString()))
                {
                    returnOrder.canCancel = true;
                }
                if (returnOrder.Status == OrderStatus.COMPLETE.ToString()
                    && !(await CheckOrderIsFeedbacked(returnOrder.Id)))
                {
                    returnOrder.canFeedback = true;
                }
                returnOrder.isPayed = bool.Parse((await CheckOrderIsPayed(returnOrder.Id)).Data.ToString());
                return new ServiceActionResult(true) { Data = returnOrder };
            }
            else
            {
                return new ServiceActionResult(false, "Order is not exits or deleted");
            }
        }

        public async Task<ServiceActionResult> GetOrderByShop(Guid id, SearchOrderRequest request)
        {
            IQueryable<Order> orderQuery = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Include(a => a.OrderItems).Include(b => b.Customer).Include(c => c.Shop).Where(x => x.ShopId == id);

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

            foreach (var order in (List<OrderResponse>)paginationResult.Data)
            {
                order.isPayed = bool.Parse((await CheckOrderIsPayed(order.Id)).Data.ToString());
            }

            return new ServiceActionResult(true) { Data = paginationResult };
        }

        public async Task<ServiceActionResult> GetOrderByUser(Guid id, SearchOrderRequest request)
        {
            IQueryable<Order> orderQuery = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Include(a => a.OrderItems).ThenInclude(d => d.Product).ThenInclude(e => e.Images).Include(b => b.Customer).Include(c => c.Shop).Where(x => x.CustomerId == id);

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

            foreach (var order in (List<OrderResponse>)paginationResult.Data)
            {
                if ((order.Status == OrderStatus.ORDERED.ToString() || order.Status == OrderStatus.CHECKING.ToString()))
                {
                    order.canCancel = true;
                }
                if (order.Status == OrderStatus.COMPLETE.ToString()
                    && !(await CheckOrderIsFeedbacked(order.Id)))
                {
                    order.canFeedback = true;
                }
                order.isPayed = bool.Parse((await CheckOrderIsPayed(order.Id)).Data.ToString());
            }

            return new ServiceActionResult(true) { Data = paginationResult };
        }

        public async Task<ServiceActionResult> GetOrderForUser(Guid userId, SearchOrderRequest request)
        {
            IQueryable<Order> orderQuery = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Include(a => a.OrderItems).Include(b => b.Customer).Include(c => c.Shop).Where(x => x.CustomerId == userId);

            if (request.Status != 0)
            {
                orderQuery = orderQuery.Where(m => m.Status.Equals(request.Status.ToString()));
            }

            orderQuery = request.IsDesc ? orderQuery.OrderByDescending(a => a.CreateDate) : orderQuery.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Order, OrderResponse>(_mapper, orderQuery, request.PageSize, request.PageIndex);

            foreach (var order in (List<OrderResponse>)paginationResult.Data)
            {
                if ((order.Status == OrderStatus.ORDERED.ToString() || order.Status == OrderStatus.CHECKING.ToString()))
                {
                    order.canCancel = true;
                }
                if (order.Status == OrderStatus.COMPLETE.ToString()
                    && !(await CheckOrderIsFeedbacked(order.Id)))
                {
                    order.canFeedback = true;
                }
                order.isPayed = bool.Parse((await CheckOrderIsPayed(order.Id)).Data.ToString());
            }

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

        public async Task<ServiceActionResult> GetTotalOrderInShop(Guid shopId, TotalOrdersRequest request)
        {
            var orders = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Where(x => x.ShopId == shopId);
            if (request.Year != 0)
            {
                if (request.Month != 0)
                {
                    if (request.Day != 0)
                    {
                        orders = orders.Where(x => x.CreateDate.Year == request.Year && x.CreateDate.Month == request.Month && x.CreateDate.Day == request.Day);
                    }
                    else
                    {
                        orders = orders.Where(x => x.CreateDate.Year == request.Year && x.CreateDate.Month == request.Month);
                    }
                }
                else
                {
                    orders = orders.Where(x => x.CreateDate.Year == request.Year);
                }
            }
            if (request.Status != 0)
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
     
            order.LastUpdateDate = DateTimeHelper.GetCurrentTime();
           
            await _unitOfWork.CommitAsync();

            return new ServiceActionResult(true) { Data = order };
        }

        private string GetBase64QRCode(byte[] QRcode)
        {
            return Convert.ToBase64String(QRcode);
        }

        private async Task<bool> CheckQRCodeExist(string qrCodeHash)
        {
            return (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable())
                .Any(x => x.QRCode == qrCodeHash);
        }

        public async Task<ServiceActionResult> CheckQRCodeOfUser(string QRcode, Guid userId)
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

        private NotificationTitle GetTitleNotification(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.ORDERED:
                    return NotificationTitle.BOOKING_ORDER;
                case OrderStatus.CHECKING:
                    return NotificationTitle.CHECKING_ORDER;
                case OrderStatus.PREPARING:
                    return NotificationTitle.PREPARING_ORDER;
                case OrderStatus.PREPARED:
                    return NotificationTitle.PREPARED_ORDER;
                case OrderStatus.TAKENOVER:
                    return NotificationTitle.TAKENOVER_ORDER;
                case OrderStatus.COMPLETE:
                    return NotificationTitle.COMPLETE_ORDER;
                case OrderStatus.CANCEL:
                    return NotificationTitle.CANCEL_ORDER;
                default:
                    return 0;
            }
        }

        public async Task<List<Order>> GetOrdersForNotificaton()
        {
            var orders = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Where(x => x.OrderDate < DateTimeHelper.GetCurrentTime().AddMinutes(30) && x.OrderDate > DateTimeHelper.GetCurrentTime().AddMinutes(-30) && x.Status.Equals(OrderStatus.CHECKING.ToString())).ToList();
            return orders;
        }

        public async Task TrainModelFromOrderHistory()
        {
            var orderItems = (await _unitOfWork.OrderItemRepository.GetAllAsyncAsQueryable()).Include(oi => oi.Order).ToList();

            var data = orderItems.Select(oi => new ProductEntry
            {
                UserId = (float)oi.Order.CustomerId.GetHashCode(),  // Convert Guid to Float
                ProductId = (float)oi.ProductId.GetHashCode(),  // Convert Guid to Float
                Label = 1 // Indicating purchase
            }).ToList();

            
            _recommendationEngine.TrainModel(data);
        }

        public async Task<ServiceActionResult> DeleteAllOrderInShop(Guid shopId)
        {
            var orders = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Where(x => x.ShopId == shopId).ToList();
            return new ServiceActionResult();
        }

        public async Task<bool> CheckOrderIsFeedbacked(Guid orderId)
        {
            var feedback = (await _unitOfWork.FeedbackRepository.GetAllAsyncAsQueryable()).Where(x => x.OrderId == orderId).Count();
            if (feedback > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<ServiceActionResult> CancelListOrder(List<Guid> orderIds)
        {
            var orders = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Where(x => orderIds.Contains(x.Id)).ToList();
            if (orders == null || orders.Count == 0) 
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Not have Orders"
                };
            }
            List<Notification> notificationList = new List<Notification>();
            foreach (Order order in orders)
            {
                order.Status = OrderStatus.CANCEL.ToString();
                notificationList.Add(
                    new Notification {
                        UserId = order.CustomerId,
                        OrderId = order.Id,
                        ShopId = order.ShopId,
                        Object = $"The shop has canceled your order because it is out of stock.",
                        Status = NotificationStatus.UnRead,
                        Title = GetTitleNotification(OrderStatus.CANCEL),
                        Destination = NotificationDestination.FORUSER
                    }
                );
                bool isPayed = bool.Parse((await CheckOrderIsPayed(order.Id)).Data.ToString());
                if (isPayed)
                {
                    var x = await _walletService.UpdateBalanceInSystem(order.CustomerId, TransactionStatus.REFUND, ((decimal)order.TotalPrice), order.Id);
                    if (x < 0)
                    {
                        return new ServiceActionResult(false)
                        {
                            Detail = "The Wallet has been deleted or not exists"
                        };
                    }
                    var y = await _walletService.UpdateBalanceAdmin(TransactionStatus.REFUND, ((decimal)order.TotalPrice));
                    if (y < 0)
                    {
                        return new ServiceActionResult(false)
                        {
                            Detail = "The System Wallet has been deleted or not exists. So The system can not refund."
                        };
                    }
                }
            }
            await _unitOfWork.OrderRepository.UpdateRangeAsync(orders);
            await _unitOfWork.CommitAsync();
            await _unitOfWork.NotificationRepository.AddRangeAsync(notificationList);
            return new ServiceActionResult(true)
            {
                Detail = "Cancel list of orders successfully"
            };
        }

        public async Task<ServiceActionResult> GetOrdersInTimeForShop(Guid shopId, GetOrdersInTimeRequest request)
        {
            IQueryable<Order> orderQuery = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Include(a => a.OrderItems).ThenInclude(d => d.Product).ThenInclude(e => e.Images)
                                            .Include(b => b.Customer).Include(c => c.Shop)
                                            .Where(x => x.ShopId == shopId && x.OrderDate >= request.DateFrom && x.OrderDate <= request.DateTo);

            if (request.Status != 0)
            {
                orderQuery = orderQuery.Where(m => m.Status.Equals(request.Status.ToString()));
            }

            orderQuery = request.IsDesc ? orderQuery.OrderByDescending(a => a.OrderDate) : orderQuery.OrderBy(a => a.OrderDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Order, OrderResponse>(_mapper, orderQuery, request.PageSize, request.PageIndex);

            foreach (var order in (List<OrderResponse>)paginationResult.Data)
            {
                order.isPayed = bool.Parse((await CheckOrderIsPayed(order.Id)).Data.ToString());
            }

            return new ServiceActionResult(true) { Data = paginationResult };
        }
    }
}
