using BMS.BLL.Models.Requests.Category;
using BMS.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.BLL.Models.Requests.Product;

namespace BMS.BLL.Services.IServices
{

    public interface IProductService
    {
        Task<ServiceActionResult> GetAllProduct(ProductRequest queryParameters);
        Task<ServiceActionResult> AddProduct(CreateProductRequest request);

        Task<ServiceActionResult> UpdateProduct(Guid id, UpdateProductRequest request);
        Task<ServiceActionResult> DeleteProduct(Guid id);
        Task<ServiceActionResult> GetProduct(Guid id);

    }
}
