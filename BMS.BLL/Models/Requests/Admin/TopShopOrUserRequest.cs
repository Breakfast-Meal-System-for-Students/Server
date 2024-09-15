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
        [Length(1, 2)]
        public int? Month = 0;
        [Length(4, 4)]
        [Range(2000, 3000)]
        public int? Year = 0;
    }
}
