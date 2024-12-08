using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

}
