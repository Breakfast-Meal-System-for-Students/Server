﻿using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.Basic;
using BMS.BLL.Models.Requests.Category;
using BMS.BLL.Models.Requests.Feedbacks;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{

    public class CategoryController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        private readonly IRegisterCategoryService _registercategoryService;

        public CategoryController(ICategoryService categoryService, IRegisterCategoryService registerCategoryService)
        {
            _categoryService = categoryService;

            _registercategoryService = registerCategoryService;
    
            _baseService = (BaseService)_categoryService;
        }


        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            
            return await ExecuteServiceLogic(
                               async () => await _categoryService.DeleteCategory(id).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromQuery] CreateCategoryRequest category)
        {
            return await ExecuteServiceLogic(
                               async () => await _categoryService.AddCategory(category).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategory([FromQuery] CategoryRequest pagingRequest)
        {
            return await ExecuteServiceLogic(
                               async () => await _categoryService.GetAllCategory(pagingRequest).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid Id, [FromQuery] UpdateCategoryRequest category)
        {
            return await ExecuteServiceLogic(
                               async () => await _categoryService.UpdateCategory(Id, category).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
    }
}
