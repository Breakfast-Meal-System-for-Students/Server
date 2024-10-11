using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Shop
{
    public class UpdateShopRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        //  public string Image { get; set; } = null!;
        public string Address { get; set; } = null!;
        //  public double Rate { get; set; }

        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }
}
