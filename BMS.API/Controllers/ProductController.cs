using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.Product;
using BMS.BLL.Services;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{

    public class ProductController : BaseApiController
    {
        private readonly IProductService _productService;


        public ProductController(IProductService productService)
        {
            _productService = productService;

            _baseService = (BaseService)_productService;
        }


        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteProduct(Guid id)
        {

            return await ExecuteServiceLogic(
                               async () => await _productService.DeleteProduct(id).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequest Product)
        {
            return await ExecuteServiceLogic(
                               async () => await _productService.AddProduct(Product).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProduct([FromQuery] ProductRequest pagingRequest)
        {
            return await ExecuteServiceLogic(
                               async () => await _productService.GetAllProduct(pagingRequest).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid Id,[FromForm] UpdateProductRequest Product)
        {
            return await ExecuteServiceLogic(
                               async () => await _productService.UpdateProduct(Id, Product).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpGet("{id}")]


        public async Task<IActionResult> GetProduct(Guid id)
        {
            return await ExecuteServiceLogic(
                async () => await _productService.GetProduct(id).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
        [HttpGet(("all-product-by-shop-id"))]


        public async Task<IActionResult> GetAllProductByShopid(Guid id, [FromQuery] ProductRequest pagingRequest)
        {
            return await ExecuteServiceLogic(
                async () => await _productService.GetAllProductByShopId(id, pagingRequest).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
    }
}
