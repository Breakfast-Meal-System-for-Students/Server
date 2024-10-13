using BMS.BLL.Models;
using BMS.BLL.Models.Requests.OpeningHour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.IServices
{
    public interface IOpeningHoursService
    {
        Task<ServiceActionResult> GetOpeningHoursForShop(Guid shopId);
        Task<ServiceActionResult> UpdateOpeningHoursForShop(UpdateOpeningHoursRequest request);
    }
}
