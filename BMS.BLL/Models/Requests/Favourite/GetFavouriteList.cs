using BMS.BLL.Models.Requests.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Favourite
{
    public class GetFavouriteList : PagingRequest
    {
        public string? ShopName { get; set; }
        public bool IsDesc { get; set; } = false;
    }
}
