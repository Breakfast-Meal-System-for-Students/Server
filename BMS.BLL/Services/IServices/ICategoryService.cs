using BMS.BLL.Models.Requests.Basic;
using BMS.BLL.Models.Requests.Feedbacks;
using BMS.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.BLL.Models.Requests.Category;

namespace BMS.BLL.Services.IServices
{

    public interface ICategoryService
    {
        Task<ServiceActionResult> GetAllCategory(CategoryRequest queryParameters);
        Task<ServiceActionResult> AddCategory(CreateCategoryRequest request);

        Task<ServiceActionResult> UpdateCategory(Guid id, UpdateCategoryRequest request);
        Task<ServiceActionResult> DeleteCategory(Guid id);

    }
}
