using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Enums
{
    public enum OrderStatus
    {
        DRAFT = 1,
        ORDERED = 2,
        PREPARING = 3,
        CHECKING = 4,
        PREPARED = 5,
        TAKENOVER = 6,
        CANCEL = 7,
        COMPLETE = 8
    }
}
