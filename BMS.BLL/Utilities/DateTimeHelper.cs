using BMS.Core.Domains.Enums;
using System;

namespace BMS.BLL.Utilities
{
    public static class DateTimeHelper
    {
        private static readonly TimeZoneInfo AppTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        public static DateTime GetCurrentTime()
        {
            //return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, AppTimeZone);
            return DateTime.UtcNow.AddHours(7);
        }

        public static DateTime GetVietNameseTime(DateTime time)
        {
            //return TimeZoneInfo.ConvertTimeFromUtc(time, AppTimeZone);
            return time.AddHours(7);
        }

        public static WeekDay GetCurrentWeekDay()
        {
            // Get current Vietnamese time
            DateTime vietnamTime = GetCurrentTime();

            // Convert to WeekDay enum
            return (WeekDay)vietnamTime.DayOfWeek;
        }


        public static WeekDay GetWeekDayFromDateTime(DateTime dateTime)
        {


            // Convert to WeekDay enum
            return (WeekDay)dateTime.DayOfWeek+1;
        }
    }
}
