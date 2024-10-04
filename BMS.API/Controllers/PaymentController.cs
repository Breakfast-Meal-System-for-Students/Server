using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.PayOs;
using BMS.BLL.Models.Requests.VnPay;
using BMS.BLL.Models.Responses.VnPay;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services;
using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;

namespace BMS.API.Controllers
{
    public class PaymentController : BaseApiController
    {
        private readonly IVnPayService _vnPayService;
        private readonly IPayOSService _payOSService;
        public PaymentController(IVnPayService vnPayService, IPayOSService payOSService)
        {
            _payOSService = payOSService;
            _vnPayService = vnPayService;
            _baseService = (BaseService)vnPayService;
        }

        [HttpPost("create-payment-url")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentUrl(VnPayRequest request)
        {
            var context = HttpContext;
            return await ExecuteServiceLogic(
               async () => await _vnPayService.CreatePaymentUrl(context, request).ConfigureAwait(false)
           ).ConfigureAwait(false);
        }

        [HttpGet("payment-callback")]
        public async Task<IActionResult> PaymentCallBack([FromQuery] VnPayResponse response)
        {
            return await ExecuteServiceLogic(
               async () => await _vnPayService.PaymentExecute(response).ConfigureAwait(false)
           ).ConfigureAwait(false);
        }

        [HttpGet("/IPN")]
        public async Task<IActionResult> IPN([FromQuery] VnPayResponse response)
        {
            return await ExecuteServiceLogic(
               async () => await _vnPayService.PaymentExecute(response, true).ConfigureAwait(false)
           ).ConfigureAwait(false);
        }

        [HttpPost("create-payment-url-payOs")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentUrlPayOs(PayOsRequest request)
        {
            return await ExecuteServiceLogic(
               async () => await _payOSService.CreatePaymentLink(request).ConfigureAwait(false)
           ).ConfigureAwait(false);
        }
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook([FromBody] WebhookType webhookBody)
        {
            return await ExecuteServiceLogic(
              async () => await _payOSService.Webhook(webhookBody).ConfigureAwait(false)
          ).ConfigureAwait(false);
        }
    }
}
