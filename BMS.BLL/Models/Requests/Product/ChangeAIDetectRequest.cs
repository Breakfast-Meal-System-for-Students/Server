using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Product
{
    public class ChangeAIDetectRequest
    {
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public AIDetectStatus Status { get; set; }
    }
}
