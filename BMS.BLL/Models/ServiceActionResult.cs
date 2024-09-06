using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models
{
    public class ServiceActionResult
    {
        public bool IsSuccess { get; set; }
        public object? Data { get; set; }
        public string? Detail { get; set; }

        public ServiceActionResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public ServiceActionResult(bool isSuccess, string detail)
        {
            IsSuccess = isSuccess;
            Detail = detail;
        }

        public ServiceActionResult() : this(true)
        {
        }
    }
}
