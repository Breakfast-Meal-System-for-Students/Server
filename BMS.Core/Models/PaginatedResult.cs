using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Models
{
    public class PaginatedResult
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long LastPage { get; set; }
        public bool IsLastPage { get; set; }
        public long Total { get; set; }
        public object? Data { get; set; }
    }
}
