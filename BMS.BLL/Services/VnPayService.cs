﻿using AutoMapper;
using BMS.BLL.Constrants;
using BMS.BLL.Helpers;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.VnPay;
using BMS.BLL.Models.Responses.VnPay;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.BLL.Utilities;
using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using BMS.Core.Exceptions;
using BMS.Core.Settings;
using BMS.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BMS.BLL.Services
{
    public class VnPayService : BaseService, IVnPayService
    {
        private readonly VNPaySettings _vnPaySettings;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IWalletService _walletService;
        public VnPayService(IOptions<VNPaySettings> vnPaySettings,
            IUnitOfWork unitOfWork,
            IMapper mapper, IHubContext<NotificationHub> hubContext, IWalletService walletService) : base(unitOfWork, mapper)
        {
            _vnPaySettings = vnPaySettings.Value;
            _hubContext = hubContext;
            _walletService = walletService;
        }

        public async Task<ServiceActionResult> CreatePaymentUrl(HttpContext context, VnPayRequest vnPayRequest)
        {
            var orderId = new Guid(vnPayRequest.OrderInfo ?? throw new BusinessRuleException("Invalid order"));
            var order = await _unitOfWork.OrderRepository.FindAsync(orderId) ?? throw new BusinessRuleException("Invalid order");
            if (order.Status.Equals(OrderStatus.COMPLETE.ToString()))
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Order is already paid"
                };
            } else if (order.Status.Equals(OrderStatus.CANCEL.ToString()))
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Order is Cancel"
                };
            }
            var tick = DateTimeHelper.GetCurrentTime().Ticks.ToString();

            var vnpay = new VnPayLibrary();

            vnpay.AddRequestData(VnPayConstansts.VERSION, _vnPaySettings.Version);
            vnpay.AddRequestData(VnPayConstansts.COMMAND, _vnPaySettings.Command);
            vnpay.AddRequestData(VnPayConstansts.TMN_CODE, _vnPaySettings.TmnCode);
            vnpay.AddRequestData(VnPayConstansts.AMOUNT, (vnPayRequest.Amount * 100).ToString());
            vnpay.AddRequestData(VnPayConstansts.CREATE_DATE, DateTimeHelper.GetCurrentTime().ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData(VnPayConstansts.CURR_CODE, _vnPaySettings.CurrencyCode);
            vnpay.AddRequestData(VnPayConstansts.IP_ADDRESS, Utils.GetIpAddress(context));
            vnpay.AddRequestData(VnPayConstansts.LOCALE, _vnPaySettings.Locale);
            vnpay.AddRequestData(VnPayConstansts.ORDER_INFOR, vnPayRequest.OrderInfo);
            vnpay.AddRequestData(VnPayConstansts.ORDER_TYPE, vnPayRequest.OrderType);
            vnpay.AddRequestData(VnPayConstansts.RETURN_URL, vnPayRequest.ReturnUrl);
            vnpay.AddRequestData(VnPayConstansts.TXN_REF, tick);

            string paymentUrl = vnpay.CreateRequestUrl(_vnPaySettings.BaseUrl, _vnPaySettings.HashSecret);
            await Task.CompletedTask;

            return new ServiceActionResult() { Data = paymentUrl };
        }

        public async Task<ServiceActionResult> CreatePaymentUrlForBuyPackage(HttpContext context, VnPayForBuyPackageRequest request)
        {

            var shop = await _unitOfWork.ShopRepository.FindAsync(request.ShopId);
            if (shop == null || shop.IsDeleted == true)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Shop is not valid or delete"
                };
            }
            var package = await _unitOfWork.PackageRepository.FindAsync(request.PackageId);
            if (package == null || package.IsDeleted == true)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Package is not valid or delete"
                };
            }
            var packageDB = (await _unitOfWork.Package_ShopRepository.GetAllAsyncAsQueryable()).Include(x => x.Package).Where(x => x.ShopId == request.ShopId && x.PackageId == request.PackageId && x.CreateDate.AddDays(x.Package.Duration) > DateTimeHelper.GetCurrentTime()).ToList();
            if (packageDB != null && packageDB.Any())
            {
                return new ServiceActionResult(false)
                {
                    Detail = "The Package is being used in Shop. You can not buy the same Package."
                };
            }
            var tick = DateTimeHelper.GetCurrentTime().Ticks.ToString();

            var vnpay = new VnPayLibrary();

            vnpay.AddRequestData(VnPayConstansts.VERSION, _vnPaySettings.Version);
            vnpay.AddRequestData(VnPayConstansts.COMMAND, _vnPaySettings.Command);
            vnpay.AddRequestData(VnPayConstansts.TMN_CODE, _vnPaySettings.TmnCode);
            vnpay.AddRequestData(VnPayConstansts.AMOUNT, (request.Amount * 100).ToString());
            vnpay.AddRequestData(VnPayConstansts.CREATE_DATE, DateTimeHelper.GetCurrentTime().ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData(VnPayConstansts.CURR_CODE, _vnPaySettings.CurrencyCode);
            vnpay.AddRequestData(VnPayConstansts.IP_ADDRESS, Utils.GetIpAddress(context));
            vnpay.AddRequestData(VnPayConstansts.LOCALE, _vnPaySettings.Locale);
            vnpay.AddRequestData(VnPayConstansts.ORDER_INFOR, $"{request.ShopId} + / + {request.PackageId}");
            vnpay.AddRequestData(VnPayConstansts.ORDER_TYPE, request.OrderType);
            vnpay.AddRequestData(VnPayConstansts.RETURN_URL, request.ReturnUrl);
            vnpay.AddRequestData(VnPayConstansts.TXN_REF, tick);

            string paymentUrl = vnpay.CreateRequestUrl(_vnPaySettings.BaseUrl, _vnPaySettings.HashSecret);
            await Task.CompletedTask;

            return new ServiceActionResult() { Data = paymentUrl };
        }

        public async Task<ServiceActionResult> CreatePaymentUrlForDeposit(HttpContext context, VNPayForDepositRequest request)
        {
            var userId = new Guid(request.UserId ?? throw new BusinessRuleException("Invalid userId"));
            var user = await _unitOfWork.UserRepository.FindAsync(userId);
            if (user == null)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "User is not exist"
                };
            }
            var tick = DateTimeHelper.GetCurrentTime().Ticks.ToString();

            var vnpay = new VnPayLibrary();

            vnpay.AddRequestData(VnPayConstansts.VERSION, _vnPaySettings.Version);
            vnpay.AddRequestData(VnPayConstansts.COMMAND, _vnPaySettings.Command);
            vnpay.AddRequestData(VnPayConstansts.TMN_CODE, _vnPaySettings.TmnCode);
            vnpay.AddRequestData(VnPayConstansts.AMOUNT, (request.Amount * 100).ToString());
            vnpay.AddRequestData(VnPayConstansts.CREATE_DATE, DateTimeHelper.GetCurrentTime().ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData(VnPayConstansts.CURR_CODE, _vnPaySettings.CurrencyCode);
            vnpay.AddRequestData(VnPayConstansts.IP_ADDRESS, Utils.GetIpAddress(context));
            vnpay.AddRequestData(VnPayConstansts.LOCALE, _vnPaySettings.Locale);
            vnpay.AddRequestData(VnPayConstansts.ORDER_INFOR, request.UserId);
            vnpay.AddRequestData(VnPayConstansts.ORDER_TYPE, request.OrderType);
            vnpay.AddRequestData(VnPayConstansts.RETURN_URL, request.ReturnUrl);
            vnpay.AddRequestData(VnPayConstansts.TXN_REF, tick);

            string paymentUrl = vnpay.CreateRequestUrl(_vnPaySettings.BaseUrl, _vnPaySettings.HashSecret);
            await Task.CompletedTask;

            return new ServiceActionResult() { Data = paymentUrl };
        }

        public async Task<ServiceActionResult> PaymentExecute(VnPayResponse response, bool isIPN = false)
        {
            /*var vnpay = new VnPayLibrary();

            Type type = response.GetType();

            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                vnpay.AddResponseData(property.Name, property.GetValue(response).ToString());
            }*/

            /*bool checkSignature = vnpay.ValidateSignature(response.vnp_SecureHash, _vnPaySettings.HashSecret);

            if (!checkSignature)
            {
                return new ServiceActionResult(false);
            }*/

            if (isIPN)
            {
                //Double.TryParse(response.vnp_Amount, out double result);
                var orderId = new Guid(response.vnp_OrderInfo ?? throw new BusinessRuleException("Invalid order"));
                var order = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Include(x => x.Transactions).FirstOrDefault(y => y.Id == orderId) ?? throw new BusinessRuleException("Invalid application");

                if (order.Status.Equals(Core.Domains.Enums.OrderStatus.COMPLETE.ToString()))
                {
                    return new ServiceActionResult(false)
                    {
                        Detail = "Order is already paid"
                    };
                } else if (order.Status.Equals(OrderStatus.CANCEL.ToString()))
                {
                    return new ServiceActionResult(false)
                    {
                        Detail = "Order is Cancel"
                    };
                }

                var isPaySucceed = response.vnp_ResponseCode?.Equals("00") ?? false;

                if (isPaySucceed)
                {
                    var y = await _walletService.UpdateBalanceAdmin(TransactionStatus.PAID, Convert.ToDecimal(response.vnp_Amount));
                    if (y < 0)
                    {
                        return new ServiceActionResult(false)
                        {
                            Detail = "The System Wallet has been deleted or not exists. So The system can not recieved."
                        };
                    }
                    // exccule logic affter payment
                    Transaction transaction = new Transaction()
                    {
                        OrderId = order.Id,
                        Price = Convert.ToDouble(response.vnp_Amount),
                        Method = TransactionMethod.VnPay.ToString(),
                        Status = TransactionStatus.PAID,
                        CreateDate = DateTimeHelper.GetCurrentTime(),
                    };

                    await _unitOfWork.TransactionRepository.AddAsync(transaction);
                    await _unitOfWork.CommitAsync();
                    List<Notification> notifications = new List<Notification>();
                    Notification notification = new Notification
                    {
                        UserId = order.CustomerId,
                        OrderId = order.Id,
                        ShopId = order.ShopId,
                        Object = $"Payment Received! The customer has successfully paid for their order.",
                        Status = NotificationStatus.UnRead,
                        Title = NotificationTitle.PAYMENT_ORDER,
                        Destination = NotificationDestination.FORUSER
                    };
                    notifications.Add(notification);
                    Notification notification1 = new Notification
                    {
                        UserId = order.CustomerId,
                        OrderId = order.Id,
                        ShopId = order.ShopId,
                        Object = $"Payment Received! The customer has successfully paid for their order.",
                        Status = NotificationStatus.UnRead,
                        Title = NotificationTitle.PAYMENT_ORDER,
                        Destination = NotificationDestination.FORSHOP
                    };
                    notifications.Add(notification1);
                    /*Notification notification2 = new Notification
                    {
                        UserId = order.CustomerId,
                        OrderId = order.Id,
                        ShopId = order.ShopId,
                        Object = $"Payment Received! The customer has successfully paid for their order.",
                        Status = NotificationStatus.UnRead,
                        Title = NotificationTitle.PAYMENT_ORDER,
                        Destination = NotificationDestination.FORSTAFF
                    };
                    notifications.Add(notification2);*/
                    await _unitOfWork.NotificationRepository.AddRangeAsync(notifications);

                    await _unitOfWork.CommitAsync();
                    /*await _hubContext.Clients.User(notification.UserId.ToString()).SendAsync("ReceiveNotification", notification.Object);
                    await _hubContext.Clients.User(notification.ShopId.ToString()).SendAsync("ReceiveNotification", notification.Object);*/


                    return new ServiceActionResult(true)
                    {
                        Data = response
                    };
                }
                else
                {
                    Transaction transaction = new Transaction()
                    {
                        OrderId = order.Id,
                        Price = Convert.ToDouble(response.vnp_Amount),
                        Method = TransactionMethod.VnPay.ToString(),
                        Status = TransactionStatus.ERROR,
                        CreateDate = DateTimeHelper.GetCurrentTime()
                    };
                    await _unitOfWork.TransactionRepository.AddAsync(transaction);
                    return new ServiceActionResult(true)
                    {
                        Data = response,
                        Detail = "Payment is Error"
                    };
                }

            }
            return new ServiceActionResult(true)
            {
                Data = response
            };


        }
    }
}
