using BMS.BLL.Models;
using BMS.BLL.Models.Requests.PayOs;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.IServices
{
    public interface IPayOSService
    {
        Task<ServiceActionResult> CreatePaymentLink(PayOsRequest request);
        Task<ServiceActionResult> Webhook(WebhookType request);

    }
}
