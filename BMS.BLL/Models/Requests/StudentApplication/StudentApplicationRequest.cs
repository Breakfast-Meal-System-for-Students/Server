using BMS.BLL.Models.Requests.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.StudentApplication
{

    public class StudentApplicationRequest : PagingRequest
    {

        public string? Search { get; set; }
        public bool IsDesc { get; set; } = false;

        public string Status { get; set; } = string.Empty;
    }
}
