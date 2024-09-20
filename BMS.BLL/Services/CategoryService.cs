using AutoMapper;
using BMS.BLL.Models.Requests.Basic;
using BMS.BLL.Models.Requests.Feedbacks;
using BMS.BLL.Models.Responses.Feedbacks;
using BMS.BLL.Models;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using BMS.Core.Helpers;
using BMS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.BLL.Models.Requests.Category;
using BMS.BLL.Models.Responses.Category;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BMS.BLL.Services
{

    public class CategoryService : BaseService, ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        public async Task<ServiceActionResult> GetAllCategory(CategoryRequest queryParameters)
        {

            IQueryable<Category> categoryQueryable = (await _unitOfWork.CategoryRepositoy.GetAllAsyncAsQueryable()).Where(a=> a.IsDeleted==false);



            if (!string.IsNullOrEmpty(queryParameters.Search))
            {
                categoryQueryable = categoryQueryable.Where(m => m.Description.Contains(queryParameters.Search));
            }

            categoryQueryable = queryParameters.IsDesc ? categoryQueryable.OrderByDescending(a => a.CreateDate) : categoryQueryable.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Category, CategoryResponse>(_mapper, categoryQueryable, queryParameters.PageSize, queryParameters.PageIndex);



            return new ServiceActionResult() { Data = paginationResult };
        }
        public async Task<ServiceActionResult> AddCategory(CreateCategoryRequest request)
        {
        
            
            var categoryEntity = _mapper.Map<Category>(request);
            if (request.Image != null)
            {
                var imageUrl = await _fileStorageService.UploadFileBlobAsync(request.Image);
                categoryEntity.Image = imageUrl;
            }
            await _unitOfWork.CategoryRepositoy.AddAsync(categoryEntity);
            // do something add feedback
            await _unitOfWork.CommitAsync();

            return new ServiceActionResult(true) { Data = categoryEntity };
        }

        public async Task<ServiceActionResult> UpdateCategory(Guid id, UpdateCategoryRequest request)
        {

           
            var category = await _unitOfWork.CategoryRepositoy.FindAsync(id) ?? throw new ArgumentNullException("Category is not exist");
            category.Description = request.Description;
            if (request.Image != null)
            {
                var imageUrl = await _fileStorageService.UploadFileBlobAsync(request.Image);
                category.Image = imageUrl;
            }
          
            category.LastUpdateDate = DateTime.UtcNow;
            category.Name = request.Name;
            await _unitOfWork.CommitAsync();

            return new ServiceActionResult(true) { Data = category };
        }



        public async Task<ServiceActionResult> DeleteCategory(Guid id)
        {
            await _unitOfWork.CategoryRepositoy.SoftDeleteByIdAsync(id);
            return new ServiceActionResult();
        }

    }
}
