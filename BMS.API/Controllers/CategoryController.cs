using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.Basic;
using BMS.BLL.Models.Requests.Category;
using BMS.BLL.Models.Requests.Feedbacks;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {

            return await ExecuteServiceLogic(
                               async () => await _categoryService.DeleteCategory(id).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryRequest category)
        {
            return await ExecuteServiceLogic(
                               async () => await _categoryService.AddCategory(category).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllCategory([FromQuery] CategoryRequest pagingRequest)
        {
            return await ExecuteServiceLogic(
                               async () => await _categoryService.GetAllCategory(pagingRequest).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetCategoryById(Guid Id)
        {
            return await ExecuteServiceLogic(
                               async () => await _categoryService.GetCategoryById(Id).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateCategory([FromForm] UpdateCategoryRequest category)
        {
            return await ExecuteServiceLogic(
                               async () => await _categoryService.UpdateCategory(category.Id, category).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
    }
}
