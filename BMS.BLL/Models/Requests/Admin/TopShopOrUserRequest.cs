using BMS.BLL.Models.Requests.Basic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Admin
{
    public class TopShopOrUserRequest : PagingRequest
    {
        [Range(1, 12)]
        public int? Month { get; set; } = 0;
        [Range(2000, 2024)]
        public int? Year { get; set; } = 0;
    }
}
