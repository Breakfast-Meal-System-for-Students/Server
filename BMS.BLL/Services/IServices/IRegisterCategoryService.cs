using BMS.BLL.Models.Requests.Category;
using BMS.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.BLL.Models.Requests.RegisterCategory;

namespace BMS.BLL.Services.IServices
{

    public interface IRegisterCategoryService
    {
        Task<ServiceActionResult> GetCategoryByProduct(Guid productId, RegisterCategoryRequest queryParameters);
        Task<ServiceActionResult> GetProductByCategory(Guid categoryId, RegisterCategoryRequest queryParameters);
        Task<ServiceActionResult> AddReCategory(CreateRegisterCategoryRequest request);

        Task<ServiceActionResult> DeleteReCategory(Guid id);

    }
}
