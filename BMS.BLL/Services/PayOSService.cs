using AutoMapper;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.PayOs;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Entities;
using BMS.Core.Settings;
using BMS.DAL;
using Microsoft.AspNetCore.SignalR;
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
    public class PayOSService(PayOS _payOS,
        IOptions<PayOSSettings> payOsSettings,
        IUnitOfWork _unitOfWork,
        IMapper _mapper,
        IHubContext<NotificationHub> _hubContext, IUserClaimsService userClaimsService) : IPayOSService
    {
        private readonly PayOSSettings _payOsSettings = payOsSettings.Value;
        private readonly UserClaims _user = userClaimsService.GetUserClaims();
        public async Task<ServiceActionResult> CreatePaymentLink(PayOsRequest request)
        {
            //TODO: something



            
              int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
            //Hard code ---> to do something(para orderdetail)
            ItemData item = new ItemData("Product 1", 1, request.Amount);
            List<ItemData> items = new List<ItemData>();
            items.Add(item);
            PaymentData paymentData = new PaymentData(orderCode, request.Amount, request.Description, items, _payOsSettings.CancelUrl, _payOsSettings.ReturnUrl);

            CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
            await _payOS.confirmWebhook(_payOsSettings.WebhookUrl);

          
            await _unitOfWork.CommitAsync();

            return new ServiceActionResult { Data = createPayment };
        }

        public async Task<ServiceActionResult> Webhook(WebhookType webhookBody)
        {
            WebhookData webhookData = _payOS.verifyPaymentWebhookData(webhookBody);

            if (webhookData != null && webhookData.code == "00")
            {
               // exxecute save order  (was success)

               //
        
                await _unitOfWork.CommitAsync();

  


                return new ServiceActionResult(true)
                {
                    Data = webhookData
                };
            }
            else
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Payment failed"
                };
            }

        }
    }
}
