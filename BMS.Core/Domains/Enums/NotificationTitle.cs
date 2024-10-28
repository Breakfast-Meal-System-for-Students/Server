using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Enums
{
    public enum NotificationTitle
    {
        BOOKING_ORDER = 1,
        CHECKING_ORDER = 2,
        PREPARING_ORDER = 3,
        PREPARED_ORDER = 4,
        TAKENOVER_ORDER = 5,
        CANCEL_ORDER = 6,
        COMPLETE_ORDER = 7,
        FEEDBACK_ORDER = 8,
        PAYMENT_ORDER = 9,
    }
}
