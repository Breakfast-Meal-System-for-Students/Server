using BMS.BLL.Models;
using BMS.Core.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.IServices
{
    public interface IRecommendationService
    {
        Task<List<OrderItem>> GetUserOrderHistory(Guid userId);
        Task<List<Product>> GetPopularProducts(int limit = 10);
        Task<ServiceActionResult> ConvertToProduct(List<Product> products);
    }
}
