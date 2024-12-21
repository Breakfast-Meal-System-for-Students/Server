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
        void AddDefaultForShop(Guid shopId, int to_hour, int from_hour, int to_minute, int from_minute);
        Task<ServiceActionResult> UpdateOpeningHoursOnceDayForShop(UpdateDayOpeningHoursRequest request);
    }
}
