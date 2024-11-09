using AutoMapper;
using BMS.BLL.Constrants;
using BMS.BLL.Helpers;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.VnPay;
using BMS.BLL.Models.Responses.VnPay;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
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

        public VnPayService(IOptions<VNPaySettings> vnPaySettings,
            IUnitOfWork unitOfWork,
            IMapper mapper) : base(unitOfWork, mapper)
        {
            _vnPaySettings = vnPaySettings.Value;
        }

        public async Task<ServiceActionResult> CreatePaymentUrl(HttpContext context, VnPayRequest vnPayRequest)
        {
            var orderId = new Guid(vnPayRequest.OrderInfo ?? throw new BusinessRuleException("Invalid order"));
            var order = await _unitOfWork.OrderRepository.FindAsync(orderId) ?? throw new BusinessRuleException("Invalid order");
            if (order.Status.Equals(OrderStatus.COMPLETE))
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Order is already paid"
                };
            } else if (order.Status.Equals(OrderStatus.CANCEL))
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Order is Cancel"
                };
            }
            var tick = DateTime.Now.Ticks.ToString();

            var vnpay = new VnPayLibrary();

            vnpay.AddRequestData(VnPayConstansts.VERSION, _vnPaySettings.Version);
            vnpay.AddRequestData(VnPayConstansts.COMMAND, _vnPaySettings.Command);
            vnpay.AddRequestData(VnPayConstansts.TMN_CODE, _vnPaySettings.TmnCode);
            vnpay.AddRequestData(VnPayConstansts.AMOUNT, (vnPayRequest.Amount * 100).ToString());
            vnpay.AddRequestData(VnPayConstansts.CREATE_DATE, DateTime.UtcNow.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData(VnPayConstansts.CURR_CODE, _vnPaySettings.CurrencyCode);
            vnpay.AddRequestData(VnPayConstansts.IP_ADDRESS, Utils.GetIpAddress(context));
            vnpay.AddRequestData(VnPayConstansts.LOCALE, _vnPaySettings.Locale);
            vnpay.AddRequestData(VnPayConstansts.ORDER_INFOR, vnPayRequest.OrderInfo);
            vnpay.AddRequestData(VnPayConstansts.ORDER_TYPE, vnPayRequest.OrderType);
            vnpay.AddRequestData(VnPayConstansts.RETURN_URL, _vnPaySettings.ReturnUrl);
            vnpay.AddRequestData(VnPayConstansts.TXN_REF, tick);

            string paymentUrl = vnpay.CreateRequestUrl(_vnPaySettings.BaseUrl, _vnPaySettings.HashSecret);
            await Task.CompletedTask;

            return new ServiceActionResult() { Data = paymentUrl };
        }

        public async Task<ServiceActionResult> PaymentExecute(VnPayResponse response, bool isIPN = false)
        {
            var vnpay = new VnPayLibrary();

            Type type = response.GetType();

            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                vnpay.AddResponseData(property.Name, property.GetValue(response).ToString());
            }

            bool checkSignature = vnpay.ValidateSignature(response.vnp_SecureHash, _vnPaySettings.HashSecret);

            if (!checkSignature)
            {
                return new ServiceActionResult(false);
            }

            if (isIPN)
            {
                Double.TryParse(response.vnp_Amount, out double result);
                var orderId = new Guid(response.vnp_OrderInfo ?? throw new BusinessRuleException("Invalid order"));
                var order = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Include(x => x.Transactions).FirstOrDefault(y => y.Id == orderId) ?? throw new BusinessRuleException("Invalid application");

                if (order.Status.Equals(Core.Domains.Enums.OrderStatus.COMPLETE))
                {
                    return new ServiceActionResult(false)
                    {
                        Detail = "Order is already paid"
                    };
                } else if (order.Status.Equals(OrderStatus.CANCEL))
                {
                    return new ServiceActionResult(false)
                    {
                        Detail = "Order is Cancel"
                    };
                }

                var isPaySucceed = (response.vnp_ResponseCode?.Equals("00") ?? false)
                    && (response.vnp_TransactionStatus?.Equals("00") ?? false);

                if (isPaySucceed)
                {
                    // exccule logic affter payment
                    Transaction transaction = new Transaction()
                    {
                        OrderId = order.Id,
                        Price = Convert.ToDouble(response.vnp_Amount),
                        Method = TransactionMethod.VnPay.ToString(),
                        Status = TransactionStatus.PAID,
                        CreateDate = DateTime.Parse(response.vnp_PayDate)
                    };

                    await _unitOfWork.TransactionRepository.AddAsync(transaction);

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

                    await _unitOfWork.NotificationRepository.AddAsync(notification);
                    notification.Destination = NotificationDestination.FORSHOP;
                    await _unitOfWork.NotificationRepository.AddAsync(notification);
                    notification.Destination = NotificationDestination.FORSTAFF;
                    await _unitOfWork.NotificationRepository.AddAsync(notification);

                    await _unitOfWork.CommitAsync();




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
                        CreateDate = DateTime.Parse(response.vnp_PayDate),
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
