using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Basic
{
    public class PagingRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public PagingRequest(int pageIndex, int pageSize) { PageIndex = pageIndex; PageSize = pageSize; }
    }
}
