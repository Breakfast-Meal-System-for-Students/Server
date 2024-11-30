using AutoMapper;
using Azure;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.PayOs;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using BMS.Core.Exceptions;
using BMS.Core.Settings;
using BMS.DAL;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services
{
    public class PayOSService : BaseService, IPayOSService
    {
        private readonly PayOSSettings _payOsSettings;
        private readonly UserClaims _user;
        private readonly PayOS _payOS;
        private readonly IHubContext<NotificationHub> _hubContext;
        public PayOSService(PayOS payOS, IOptions<PayOSSettings> payOsSettings, IUserClaimsService userClaimsService, IUnitOfWork unitOfWork, IMapper mapper, IHubContext<NotificationHub> hubContext) : base(unitOfWork, mapper)
        {
            _payOS = payOS;
            _payOsSettings = payOsSettings.Value;
            _user = userClaimsService.GetUserClaims();
            _hubContext = hubContext;
        }

        public async Task<ServiceActionResult> CreatePaymentLink(PayOsRequest request)
        {
            //TODO: something
            var orderId = request.OrderCode;
            var order = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Include(x => x.Transactions).FirstOrDefault(y => y.Id == orderId) ?? throw new BusinessRuleException("Invalid order");
            if (order.Status.Equals(OrderStatus.COMPLETE.ToString()))
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Order is already paid"
                };
            }
            else if (order.Status.Equals(OrderStatus.CANCEL.ToString()))
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Order is Cancel"
                };
            }



            int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
            //Hard code ---> to do something(para orderdetail)
            ItemData item = new ItemData("Product 1", 1, request.Amount);
            List<ItemData> items = new List<ItemData>();
            items.Add(item);
            request.Description = request.OrderCode.ToString() + ":" + request.Description;
            PaymentData paymentData = new PaymentData(orderCode, request.Amount, request.Description, items, _payOsSettings.CancelUrl, _payOsSettings.ReturnUrl);

            CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
            await _payOS.confirmWebhook(_payOsSettings.WebhookUrl);

          
            await _unitOfWork.CommitAsync();

            return new ServiceActionResult { Data = createPayment };
        }

        public async Task<ServiceActionResult> Webhook(WebhookType webhookBody)
        {
            WebhookData webhookData = _payOS.verifyPaymentWebhookData(webhookBody);
            var orderId = new Guid(webhookBody.data.description.Split(':')[0] ?? throw new Exception("Invalid order"));
            var order = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Include(x => x.Transactions).FirstOrDefault(y => y.Id == orderId) ?? throw new BusinessRuleException("Invalid order");
            if (order.Status.Equals(OrderStatus.COMPLETE.ToString()))
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Order is already paid"
                };
            }
            else if (order.Status.Equals(OrderStatus.CANCEL.ToString()))
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Order is Cancel"
                };
            }

            if (webhookData != null && webhookData.code == "00")
            {
                
                // exxecute save order  (was success)
                Core.Domains.Entities.Transaction transaction = new Core.Domains.Entities.Transaction()
                {
                    OrderId = order.Id,
                    Price = Convert.ToDouble(webhookBody.data.amount),
                    Method = TransactionMethod.PayOs.ToString(),
                    Status = TransactionStatus.PAID,
                    CreateDate = DateTime.Now,
                };

                await _unitOfWork.TransactionRepository.AddAsync(transaction);

                List<Notification> notifications = new List<Notification>();
                Notification notification = new Notification
                {
                    UserId = order.CustomerId,
                    OrderId = order.Id,
                    ShopId = order.ShopId,
                    Object = $"Pay Order ID ${order.Id} sucessfully",
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
                    Object = $"Pay Order ID ${order.Id} sucessfully",
                    Status = NotificationStatus.UnRead,
                    Title = NotificationTitle.PAYMENT_ORDER,
                    Destination = NotificationDestination.FORSHOP
                };
                notifications.Add(notification1);
                Notification notification2 = new Notification
                {
                    UserId = order.CustomerId,
                    OrderId = order.Id,
                    ShopId = order.ShopId,
                    Object = $"Pay Order ID ${order.Id} sucessfully",
                    Status = NotificationStatus.UnRead,
                    Title = NotificationTitle.PAYMENT_ORDER,
                    Destination = NotificationDestination.FORSTAFF
                };
                notifications.Add(notification2);
                await _unitOfWork.NotificationRepository.AddRangeAsync(notifications);

                await _unitOfWork.CommitAsync();

                await _hubContext.Clients.User(notification.UserId.ToString()).SendAsync("ReceiveNotification", notification.Object);
                await _hubContext.Clients.User(notification.ShopId.ToString()).SendAsync("ReceiveNotification", notification.Object);

                return new ServiceActionResult(true)
                {
                    Data = webhookData
                };
            }
            else
            {
                Core.Domains.Entities.Transaction transaction = new Core.Domains.Entities.Transaction()
                {
                    OrderId = order.Id,
                    Price = Convert.ToDouble(webhookBody.data.amount),
                    Method = TransactionMethod.VnPay.ToString(),
                    Status = TransactionStatus.ERROR,
                    CreateDate = DateTime.Now,
                };
                await _unitOfWork.TransactionRepository.AddAsync(transaction);
                return new ServiceActionResult(false)
                {
                    Detail = "Payment failed"
                };
            }

        }
    }
}
