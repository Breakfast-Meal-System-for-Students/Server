using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Utilities
{
    public class RatingHelper
    {
        public static double CaculateRaingForShopWhenHaveNewFeedback(double shopRating, double newRating, int count, int initialWeight = 5)
        {
            const double defaultRating = 5.0;

            int totalFeedback = count + initialWeight;

            double totalRating = (shopRating * count) + (defaultRating * initialWeight) + newRating;

            double updatedRating = totalRating / (totalFeedback + 1);

            updatedRating = Math.Max(1, Math.Min(5, updatedRating));

            return updatedRating;
        }
    }
}
