using BMS.BLL.Models.Requests.Basic;
using BMS.Core.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Package
{

    public class PackageRequest : PagingRequest
    {
      
        public string? Search { get; set; }
        public bool IsDesc { get; set; } = false;
    }
}
