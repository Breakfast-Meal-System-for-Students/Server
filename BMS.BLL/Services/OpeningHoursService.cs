using AutoMapper;
using Azure.Core;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.OpeningHour;
using BMS.BLL.Models.Responses.Admin;
using BMS.BLL.Models.Responses.OpeningHour;
using BMS.BLL.Models.Responses.Users;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.BLL.Utilities;
using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using BMS.Core.Helpers;
using BMS.DAL;
using Microsoft.Extensions.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services
{
    public class OpeningHoursService : BaseService, IOpeningHoursService
    {
        public const int DEFAULT_FROM_HOUR = 5;
        public const int DEFAULT_TO_HOUR = 12;
        public const int DEFAULT_FROM_MINUTE = 00;
        public const int DEFAULT_TO_MINUTE = 00;

        public OpeningHoursService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<ServiceActionResult> GetOpeningHoursForShop(Guid shopId)
        {
            var openingHours = (await _unitOfWork.OpeningHoursRepository.GetAllAsyncAsQueryable()).Where(x => x.ShopId == shopId);
            var paginationResult = PaginationHelper
            .BuildPaginatedResult<OpeningHours, GetOpeningHoursForShopResonse>(_mapper, openingHours, 7, 1);

            return new ServiceActionResult(true) { Data = paginationResult };
        }

        public async Task<ServiceActionResult> UpdateOpeningHoursForShop(UpdateOpeningHoursRequest request)
        {
            var openingHours = (await _unitOfWork.OpeningHoursRepository.GetAllAsyncAsQueryable()).Where(x => x.ShopId == request.shopId).ToList();
            if (openingHours.Any()) 
            {
                foreach (OpeningHoursRequest openingHour in request.listOpeningHours)
                {
                    //var op = _mapper.Map<OpeningHours>(openingHour);

                    //op.ShopId = request.shopId;
                    openingHours.SingleOrDefault(x => x.day == openingHour.day).Set(openingHour.from_hour, openingHour.to_hour, openingHour.from_minute, openingHour.to_minute);
                }
                await _unitOfWork.OpeningHoursRepository.UpdateRangeAsync(openingHours);
                await _unitOfWork.CommitAsync();
                return new ServiceActionResult(true)
                {
                    Detail = "Update OpeningHoursForShop Successfully"
                };
            }
            return new ServiceActionResult(true)
            {
                Detail = "Shop is not existed or deleted"
            };
        }

        public async Task<ServiceActionResult> UpdateOpeningHoursOnceDayForShop(UpdateDayOpeningHoursRequest request)
        {
            var openingHours = await _unitOfWork.OpeningHoursRepository.FindAsync(x => x.Id == request.Id);
            // Validate input times
            if (request.from_hour < 5 || request.from_hour > 12 ||
                request.to_hour < 5 || request.to_hour > 12 ||
                request.from_minute < 0 || request.from_minute > 60 ||
                request.to_minute < 0 || request.to_minute > 60)
            {
                return new ServiceActionResult(false)
                {
                    Data = "Invalid time values. Hours must be between 5 and 12, and minutes must be between 0 and 60."
                };
            }
            // Get the current day of the week
            WeekDay currentDay = DateTimeHelper.GetCurrentWeekDay();
            // Converts DayOfWeek to WeekDay enum

            // Check if the current day matches the day of the opening hours
            if (openingHours.day == currentDay)
            {
                return new ServiceActionResult(true)
                {
                    Detail = "Time is already the current time in the opening hours (48 hour). No update required."
                };
            }
            if (openingHours.day == currentDay +1)
            {
                return new ServiceActionResult(true)
                {
                    Detail = "Time is already the current time in the opening hours (48 hour). No update required."
                };
            }
            // Ensure "from time" is less than "to time"
            var fromTime = new TimeSpan(request.from_hour, request.from_minute, 0);
            var toTime = new TimeSpan(request.to_hour, request.to_minute, 0);

            if (fromTime >= toTime)
            {
                return new ServiceActionResult(false)
                {
                    Data = "Invalid time range. 'From time' must be earlier than 'To time'."
                };
            }
            if (openingHours is not null)
            {

                openingHours.from_hour = request.from_hour;
                openingHours.to_hour = request.to_hour;
                openingHours.from_minute = request.from_minute;
                openingHours.to_minute = request.to_minute;

                await _unitOfWork.CommitAsync();
                return new ServiceActionResult(true)
                {
                    Detail = "Update OpeningHoursForShop Successfully"
                };
            }
            return new ServiceActionResult(true)
            {
                Detail = "Shop is not existed or deleted"
            };
        }

        public async Task AddDefaultForShop(Guid shopId,int to_hour,int from_hour, int to_minute, int from_minute)
        {
            foreach (WeekDay day in Enum.GetValues(typeof(WeekDay)))
            {
                OpeningHours openingHours = new OpeningHours()
                {
                    ShopId = shopId,
                    to_hour = to_hour,
                    from_hour = from_hour,
                    to_minute = to_minute,
                    from_minute = from_minute,
                    isOpenToday = true,
                    day = day, // Assign the current day of the week
                };
                await _unitOfWork.OpeningHoursRepository.AddAsync(openingHours);
            }

            // Commit all changes to the database
            await _unitOfWork.CommitAsync();
        }

        public async Task<ServiceActionResult> UpdateOpenTodayForShop(Guid id, bool isOpenToday)
        {
            var openingHours = await _unitOfWork.OpeningHoursRepository.FindAsync(x => x.Id == id);
            if (openingHours is not null)
            {
                // Get the current day of the week
                WeekDay currentDay = DateTimeHelper.GetCurrentWeekDay();
                // Converts DayOfWeek to WeekDay enum

                // Check if the current day matches the day of the opening hours
                if (openingHours.day == currentDay)
                {
                    return new ServiceActionResult(true)
                    {
                        Detail = "Time is already the current time in the opening hours (48 hour). No update required."
                    };
                }
                if (openingHours.day == currentDay + 1)
                {
                    return new ServiceActionResult(true)
                    {
                        Detail = "Time is already the current time in the opening hours (48 hour). No update required."
                    };
                }
                openingHours.isOpenToday = isOpenToday;
          

                await _unitOfWork.CommitAsync();
                return new ServiceActionResult(true)
                {
                    Detail = "Update OpeningHoursForShop Successfully"
                };
            }
            return new ServiceActionResult(true)
            {
                Detail = "Shop is not existed or deleted"
            };
        }


        public async Task<bool> IsWithinOpeningHours(Guid shopId, DateTime timeOrder)
        {
            // Get the current day based on the Vietnamese timezone
            WeekDay currentDay = DateTimeHelper.GetCurrentWeekDay();

            // Fetch opening hours for the shop and the current day
            var openingHours = await _unitOfWork.OpeningHoursRepository
                .FindAsync(x => x.ShopId == shopId && x.day == currentDay);

            if (openingHours == null)
            {
                // Return false if no opening hours are found for the shop on the current day
                return false;
            }

            // Calculate the start and end times for the shop's opening hours
            DateTime startTime = timeOrder.Date.AddHours(openingHours.from_hour)
                                        .AddMinutes(openingHours.from_minute);
            DateTime endTime = timeOrder.Date.AddHours(openingHours.to_hour)
                                      .AddMinutes(openingHours.to_minute);

            // Check if the provided timeOrder is within the range
            return timeOrder >= startTime && timeOrder <= endTime;
        }

    }
}
