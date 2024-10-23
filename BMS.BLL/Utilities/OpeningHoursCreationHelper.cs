using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Utilities
{
    public class OpeningHoursCreationHelper
    {
        public static List<OpeningHours> GenerateOpeningHours(Guid shopId)
        {
            List<OpeningHours> list = new List<OpeningHours>();
            OpeningHours openingHour;
            for (var i = 1; i <= 7; i++)
            {
                openingHour = new OpeningHours();
                switch (i)
                {
                    case 1:
                        openingHour.day = WeekDay.Sunday;
                        openingHour.from_hour = 6;
                        openingHour.from_minute = 0;
                        openingHour.to_hour = 11;
                        openingHour.to_minute = 0;
                        break;
                    case 2:
                        openingHour.day = WeekDay.Monday;
                        openingHour.from_hour = 6;
                        openingHour.from_minute = 0;
                        openingHour.to_hour = 11;
                        openingHour.to_minute = 0;
                        break;
                    case 3:
                        openingHour.day = WeekDay.Tuesday;
                        openingHour.from_hour = 6;
                        openingHour.from_minute = 0;
                        openingHour.to_hour = 11;
                        openingHour.to_minute = 0;
                        break;
                    case 4:
                        openingHour.day = WeekDay.Wednesday;
                        openingHour.from_hour = 6;
                        openingHour.from_minute = 0;
                        openingHour.to_hour = 11;
                        openingHour.to_minute = 0;
                        break;
                    case 5:
                        openingHour.day = WeekDay.Thusday;
                        openingHour.from_hour = 6;
                        openingHour.from_minute = 0;
                        openingHour.to_hour = 11;
                        openingHour.to_minute = 0;
                        break;
                    case 6:
                        openingHour.day = WeekDay.Friday;
                        openingHour.from_hour = 6;
                        openingHour.from_minute = 0;
                        openingHour.to_hour = 11;
                        openingHour.to_minute = 0;
                        break;
                    case 7:
                        openingHour.day = WeekDay.Saturday;
                        openingHour.from_hour = 6;
                        openingHour.from_minute = 0;
                        openingHour.to_hour = 11;
                        openingHour.to_minute = 0;
                        break;
                }
                openingHour.ShopId = shopId;
                list.Add(openingHour);
            }
            return list;
        }
    }
}
