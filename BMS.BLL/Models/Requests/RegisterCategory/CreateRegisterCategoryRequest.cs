using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.RegisterCategory
{
    public class CreateRegisterCategoryRequest
    {
        public Guid CategoryId { get; set; } 

      //  public Guid ShopId { get; set; } 

        public Guid ProductId { get; set; }
    }
}
