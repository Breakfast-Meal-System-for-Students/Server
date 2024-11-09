using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.Favourite
{
    public class FavouriteResponse
    {
        public Guid UserId { get; set; }
        public Guid ShopId { get; set; }
        public string ShopName { get; set; } = null!;
        public string? ShopImage { get; set; } = null!;
    }
}
