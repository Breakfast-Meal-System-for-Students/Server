using AutoMapper;
using BMS.BLL.Models.Requests.Category;
using BMS.BLL.Models.Responses.Category;
using BMS.BLL.Models;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Entities;
using BMS.Core.Helpers;
using BMS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.BLL.Models.Requests.RegisterCategory;
using BMS.BLL.Models.Responses.RegisterCategory;
using Microsoft.EntityFrameworkCore;

namespace BMS.BLL.Services
{

    public class RegisterCategoryService : BaseService, IRegisterCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        public RegisterCategoryService(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }


        public async Task<ServiceActionResult> GetCategoryByProduct(Guid productId,RegisterCategoryRequest queryParameters)
        {

            IQueryable<RegisterCategory> reCategoryQueryable = (await _unitOfWork.RegisterCategoryRepository.GetAllAsyncAsQueryable()).Where(a => a.ProductId == productId).Include(a => a.Category);


            if (!string.IsNullOrEmpty(queryParameters.Search))
            {
             //   categoryQueryable = categoryQueryable.Where(m => m.Description.Contains(queryParameters.Search));
            }

            reCategoryQueryable = queryParameters.IsDesc ? reCategoryQueryable.OrderByDescending(a => a.CreateDate) : reCategoryQueryable.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<RegisterCategory, RegisterCategoryResponse>(_mapper, reCategoryQueryable, queryParameters.PageSize, queryParameters.PageIndex);



            return new ServiceActionResult() { Data = paginationResult };
        }
        public async Task<ServiceActionResult> GetProductByCategory(Guid categoryId, RegisterCategoryRequest queryParameters)
        {

            IQueryable<RegisterCategory> reCategoryQueryable = (await _unitOfWork.RegisterCategoryRepository.GetAllAsyncAsQueryable()).Where(a => a.CategoryId == categoryId).Include(a=>a.Product).ThenInclude(a=>a.Images);


            if (!string.IsNullOrEmpty(queryParameters.Search))
            {
                //   categoryQueryable = categoryQueryable.Where(m => m.Description.Contains(queryParameters.Search));
            }

            reCategoryQueryable = queryParameters.IsDesc ? reCategoryQueryable.OrderByDescending(a => a.CreateDate) : reCategoryQueryable.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<RegisterCategory, RegisterCategoryResponse>(_mapper, reCategoryQueryable, queryParameters.PageSize, queryParameters.PageIndex);



            return new ServiceActionResult() { Data = paginationResult };
        }
        public async Task<ServiceActionResult> AddReCategory(CreateRegisterCategoryRequest request)
        {
            var product = (await _unitOfWork.ProductRepository.GetAllAsyncAsQueryable()).Where(x => x.Id == request.ProductId && x.IsDeleted == false).SingleOrDefault();
            if (product == null)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Product is not Exists Or Deleted"
                };
            }
            var category = (await _unitOfWork.CategoryRepositoy.GetAllAsyncAsQueryable()).Where(x => x.Id == request.CategoryId && x.IsDeleted == false).SingleOrDefault();
            if (category == null)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Category is not Exists Or Deleted"
                };
            }
            var reCategoryEntity = _mapper.Map<RegisterCategory>(request);
            
            await _unitOfWork.RegisterCategoryRepository.AddAsync(reCategoryEntity);

            return new ServiceActionResult(true) { Data = reCategoryEntity };
        }





        public async Task<ServiceActionResult> DeleteReCategory(Guid id)
        {
            await _unitOfWork.RegisterCategoryRepository.DeleteAsync(id);
            return new ServiceActionResult();
        }

    }
}
