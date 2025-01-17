﻿using BMS.BLL.Models;
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
        Task AddDefaultForShop(Guid shopId, int to_hour, int from_hour, int to_minute, int from_minute);
        Task<ServiceActionResult> UpdateOpeningHoursOnceDayForShop(UpdateDayOpeningHoursRequest request);
        Task<ServiceActionResult> UpdateOpenTodayForShop(Guid id, bool isOpenToday);
        Task<bool> IsWithinOpeningHours(Guid shopId, DateTime timeOrder);
        Task<ServiceActionResult> UpdateOpeningHoursForShopByAdmin(UpdateOpeningHoursRequest request);
    }
}
