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
using BMS.BLL.Models;

namespace BMS.API.Controllers
{
    public class PaymentController : BaseApiController
    {
        private readonly IVnPayService _vnPayService;
        private readonly IPayOSService _payOSService;
        private readonly IUserClaimsService _userClaimsService;
        private UserClaims _userClaims;
        public PaymentController(IVnPayService vnPayService, IPayOSService payOSService, IUserClaimsService userClaimsService)
        {
            _payOSService = payOSService;
            _vnPayService = vnPayService;
            _baseService = (BaseService)vnPayService;
            _userClaimsService = userClaimsService;
            _userClaims = userClaimsService.GetUserClaims();
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

        [HttpPost("create-payment-url-forbuypackage")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentUrlForBuyPackage(VnPayForBuyPackageRequest request)
        {
            var context = HttpContext;
            return await ExecuteServiceLogic(
               async () => await _vnPayService.CreatePaymentUrlForBuyPackage(context, request).ConfigureAwait(false)
           ).ConfigureAwait(false);
        }

        [HttpPost("create-payment-url-fordeposit")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentUrlForBuyPackage(VNPayForDepositRequest request)
        {
            var context = HttpContext;
            return await ExecuteServiceLogic(
               async () => await _vnPayService.CreatePaymentUrlForDeposit(context, request).ConfigureAwait(false)
           ).ConfigureAwait(false);
        }

        [HttpPost("payment-callback")]
        public async Task<IActionResult> PaymentCallBack([FromBody] VnPayResponse response)
        {
            return await ExecuteServiceLogic(
               async () => await _vnPayService.PaymentExecute(response, true).ConfigureAwait(false)
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
