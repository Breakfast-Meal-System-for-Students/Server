using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Admin
{
    public class TotalTRansactionRequest
    {
        [Range(1, 31)]
        [Length(1, 2)]
        public int? Day = 0;
        [Range(1, 12)]
        [Length(1, 2)]
        public int? Month = 0;
        [Length(4, 4)]
        [Range(2000, 3000)]
        public int? Year = 0;
        public TransactionStatus? Status { get; set; } = 0;
    }
}
