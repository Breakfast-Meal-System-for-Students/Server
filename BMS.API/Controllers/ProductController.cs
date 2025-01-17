﻿using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.Product;
using BMS.BLL.Services;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Constants;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("AddProductToStaff")]
        public async Task<IActionResult> AddProductToStaff([FromForm] CreateProductRequest Product)
        {
            return await ExecuteServiceLogic(
                               async () => await _productService.AddProductToStaff(Product).ConfigureAwait(false)
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

        [HttpPut(("ChangeOutOfStock"))]
        [Authorize(Roles = UserRoleConstants.SHOP)]
        public async Task<IActionResult> ChangeOutOfStock([FromForm]Guid productId)
        {
            return await ExecuteServiceLogic(
                async () => await _productService.ChangeOutOfStock(productId).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpPut(("ChangeAICanDetect"))]
        [Authorize(Roles = UserRoleConstants.STAFF)]
        public async Task<IActionResult> ChangeAICanDetect([FromForm] ChangeAIDetectRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _productService.ChangeAICanDetect(request.ProductId, request.Status).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet(("GetProductBestSellerInShop"))]
        public async Task<IActionResult> GetProductBestSellerInShop(Guid shopId, [FromQuery] ProductBestSellerRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _productService.GetProductBestSellerInShop(shopId, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet(("GetProductToPreparingForShopInTime/{shopId}"))]
        [Authorize(Roles = UserRoleConstants.SHOP)]
        public async Task<IActionResult> GetProductToPreparingForShopInTime(Guid shopId, [FromQuery] GetProductToPreparingRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _productService.GetProductToPreparingForShopInTime(shopId, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
    }
}
