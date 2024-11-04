using BMS.API.Controllers.Base;
using BMS.BLL.AI;
using BMS.BLL.Models;
using BMS.BLL.Services;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationController : BaseApiController
    {
        private readonly IRecommendationService _recommendationService;
        private readonly RecommendationEngine _recommendationEngine;

        public RecommendationController(IRecommendationService recommendationService, RecommendationEngine recommendationEngine)
        {
            _recommendationService = recommendationService;
            _recommendationEngine = recommendationEngine;
            _baseService = (BaseService)recommendationService;
        }

        [HttpGet("recommend1/{userId}")]
        public async Task<IActionResult> GetRecommendations1(Guid userId)
        {
            var orderItems = await _recommendationService.GetUserOrderHistory(userId);
            var allProducts = await _recommendationService.GetPopularProducts();

            var predictions = _recommendationEngine.PredictForUser(userId.GetHashCode(), allProducts.Select(p => p.Id.GetHashCode()));

            var recommendedProducts = predictions
                .OrderByDescending(p => p.Score)
                .Take(10)
                .Select(p => allProducts.First(product => (float)product.Id.GetHashCode() == p.ProductId))
                .ToList();
            var response = await _recommendationService.ConvertToProduct(recommendedProducts).ConfigureAwait(false);
            return await ExecuteServiceLogic(() => Task.FromResult(response)).ConfigureAwait(false);
        }
        [HttpGet("recommend2/{userId}")]
        public async Task<IActionResult> GetRecommendations2(Guid userId)
        {
            var orderItems = await _recommendationService.GetUserOrderHistory(userId);

            var allProducts = await _recommendationService.GetPopularProducts();

            var purchasedProductIds = orderItems.Select(oi => oi.ProductId).ToList();

            var productsToRecommend = allProducts
                .Where(p => purchasedProductIds.Contains(p.Id)) 
                .Select(p => p.Id.GetHashCode())
                .ToList();

            var predictions = _recommendationEngine.PredictForUser(userId.GetHashCode(), productsToRecommend);

            var recommendedProducts = predictions
                .OrderByDescending(p => p.Score)
                .Take(10)
                .Select(p => allProducts.First(product => (float)product.Id.GetHashCode() == p.ProductId))
                .ToList();

            var response = await _recommendationService.ConvertToProduct(recommendedProducts).ConfigureAwait(false);
            return await ExecuteServiceLogic(() => Task.FromResult(response)).ConfigureAwait(false);
        }
    }

}
