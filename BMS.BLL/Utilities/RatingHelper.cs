using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Utilities
{
    public class RatingHelper
    {
        public static double CaculateRaingForShopWhenHaveNewFeedback(double shopRating, double newRating, int count)
        {
            double rate = 0;
            if ((shopRating == 5 && newRating == 5) || (shopRating == 1 && newRating == 1))
            {
                return shopRating;
            }
            if(shopRating > newRating)
            {
                if (count < 10)
                {
                    rate = ((shopRating * count) - newRating * 0.3) / (count + 1);
                } else if (count < 50)
                {
                    rate = ((shopRating * count) - newRating * 0.5) / (count + 1);
                } else if (count < 100)
                {
                    rate = ((shopRating * count) - newRating * 0.8) / (count + 1);
                } else
                {
                    rate = ((shopRating * count) - newRating * 1) / (count + 1);
                }
            } else
            {
                rate = ((shopRating * count) + newRating * 1) / (count + 1);
            }
            rate = rate > 5 ? 5 : rate;
            rate = rate < 1 ? 1 : rate;
            return rate;
        }
    }
}
