using AutoMapper;
using BMS.BLL.Models;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Entities;
using BMS.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services
{
    public class RecommendationService : BaseService, IRecommendationService
    {
        public RecommendationService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<ServiceActionResult> ConvertToProduct(List<Product> products)
        {
            return new ServiceActionResult() 
            { 
                Data = products.Select(p => new { p.Id, p.Name }) 
            };
        }

        public async Task<List<Product>> GetPopularProducts(int limit = 10)
        {
            var popularProducts = (await _unitOfWork.OrderItemRepository.GetAllAsyncAsQueryable()).Include(x => x.Product)
                                    .GroupBy(oi => oi.ProductId)
                                    .OrderByDescending(g => g.Count())
                                    .Take(limit)
                                    .Select(g => g.FirstOrDefault().Product)
                                    .ToList();
            return popularProducts;
        }


        public async Task<List<OrderItem>> GetUserOrderHistory(Guid userId)
        {
            var userOrderHistory = (await _unitOfWork.OrderItemRepository.GetAllAsyncAsQueryable())
                                    .Include(oi => oi.Product)
                                    .Include(oi => oi.Order)
                                    .Where(oi => oi.Order.CustomerId == userId)
                                    .ToList();
            return userOrderHistory;
        }
    }
}
