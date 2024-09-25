using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.VnPay;
using BMS.BLL.Models.Responses.VnPay;
using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
    public class PaymentController(IVnPayService _vnPayService) : BaseApiController
    {

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


    }
}
