﻿using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.Category;
using BMS.BLL.Models.Requests.RegisterCategory;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{

    public class RegisterCategoryController : BaseApiController
    {
        private readonly IRegisterCategoryService _registerCategoryService;


        public RegisterCategoryController(IRegisterCategoryService registercategoryService)
        {
            _registerCategoryService = registercategoryService;

            _baseService = (BaseService)_registerCategoryService;
        }


        [HttpDelete]

        public async Task<IActionResult> DeleteRegisterCategory([FromBody] CreateRegisterCategoryRequest category)
        {

            return await ExecuteServiceLogic(
                               async () => await _registerCategoryService.DeleteReCategory(category).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRegisterCategory([FromBody] CreateRegisterCategoryRequest category)
        {
            return await ExecuteServiceLogic(
                               async () => await _registerCategoryService.AddReCategory(category).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpGet("GetAllCategorybyProductId")]
        public async Task<IActionResult> GetAllCategorybyProductId(Guid productId, [FromQuery] RegisterCategoryRequest pagingRequest)
        {
            return await ExecuteServiceLogic(
                               async () => await _registerCategoryService.GetCategoryByProduct(productId, pagingRequest).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpGet("GetAllProductByCategory")]
        public async Task<IActionResult> GetAllProductByCategory(Guid categoryId, [FromQuery] RegisterCategoryRequest pagingRequest)
        {
            return await ExecuteServiceLogic(
                               async () => await _registerCategoryService.GetProductByCategory(categoryId, pagingRequest).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
    }
}
