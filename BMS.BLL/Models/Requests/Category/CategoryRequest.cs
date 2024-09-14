using BMS.BLL.Models.Requests.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Category
{
    public class CategoryRequest : PagingRequest
    {
       
        public string? Search { get; set; }
        public bool IsDesc { get; set; } = false;
    }
}
