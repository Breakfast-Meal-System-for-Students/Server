using BMS.BLL.Models;
using BMS.BLL.Models.Requests.VnPay;
using BMS.BLL.Models.Responses.VnPay;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.IServices
{
    public interface IVnPayService
    {
        Task<ServiceActionResult> CreatePaymentUrl(HttpContext context, VnPayRequest request);
        Task<ServiceActionResult> CreatePaymentUrlForBuyPackage(HttpContext context, VnPayForBuyPackageRequest request);
        Task<ServiceActionResult> CreatePaymentUrlForDeposit(HttpContext context, VNPayForDepositRequest request);
        Task<ServiceActionResult> PaymentExecute(VnPayResponse response, bool isIPN = false);
    }
}
