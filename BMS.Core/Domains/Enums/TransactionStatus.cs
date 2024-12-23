using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Enums
{
    public enum TransactionStatus
    {
        PAID = 1,
        PAIDTOSHOP = 2,
        ERROR = 3,
        REFUND = 4,
        DEPOSIT = 5,
        WITHDRA = 6,
    }

}
