using BMS.BLL.Models.Responses.Category;
using BMS.BLL.Models.Responses.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.RegisterCategory
{
    public class RegisterCategoryResponse
    {
        public Guid CategoryId { get; set; } // Foreign key to Category
        public CategoryResponse? Category { get; set; } = null!;

        public Guid ShopId { get; set; } // Foreign key to Shop
      //  public Shop? Shop { get; set; } = null!;

        public ProductResponse? Product { get; set; } = null!;
        public Guid ProductId { get; set; }
    }

}
