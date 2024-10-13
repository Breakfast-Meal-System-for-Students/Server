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
        public int? Day { get; set; } = 0;
        public int? Month { get; set; } = 0;
        public int? Year { get; set; } = 0;
        public TransactionStatus? Status { get; set; } = 0;
    }
}
