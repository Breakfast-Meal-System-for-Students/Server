using AutoMapper;
using BMS.BLL.Models.Requests.Product;
using BMS.BLL.Models.Responses.Product;
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
using BMS.BLL.Models.Requests.Product;
using Microsoft.EntityFrameworkCore;
using BMS.BLL.Models.Responses.Package;

namespace BMS.BLL.Services
{

    public class ProductService : BaseService, IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        public async Task<ServiceActionResult> GetAllProduct(ProductRequest queryParameters)
        {

            IQueryable<Product> ProductQueryable = (await _unitOfWork.ProductRepository.GetAllAsyncAsQueryable()).Where(a => a.IsDeleted == false).Include(a => a.Images);



            if (!string.IsNullOrEmpty(queryParameters.Search))
            {
                ProductQueryable = ProductQueryable.Where(m => m.Description.Contains(queryParameters.Search));
            }

            ProductQueryable = queryParameters.IsDesc ? ProductQueryable.OrderByDescending(a => a.CreateDate) : ProductQueryable.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Product, ProductResponse>(_mapper, ProductQueryable, queryParameters.PageSize, queryParameters.PageIndex);



            return new ServiceActionResult() { Data = paginationResult };
        }
        public async Task<ServiceActionResult> AddProduct(CreateProductRequest request)
        {
            // Map request to Product entity
            var productEntity = _mapper.Map<Product>(request);

            // Handle image upload and create Image entities
            if (request.Images != null && request.Images.Any())
            {
                foreach (var imageFile in request.Images)
                {
                    // Upload image to file storage (assuming the request.Images is a list of IFormFile)
                    var imageUrl = await _fileStorageService.UploadFileBlobAsync(imageFile);

                    // Create Image entity and associate with the Product
                    var imageEntity = new Image
                    {
                        Url = imageUrl,
                        Product = productEntity // Associate the image with the product
                    };

                    // Add the image entity to the Product's Images collection
                    productEntity.Images.Add(imageEntity);
                }
            }

            // Add the Product to the repository
            await _unitOfWork.ProductRepository.AddAsync(productEntity);

            // Commit the transaction
            await _unitOfWork.CommitAsync();

            // Return success result with the created product data
            return new ServiceActionResult(true) { Data = productEntity };
        }


        public async Task<ServiceActionResult> UpdateProduct(Guid id, UpdateProductRequest request)
        {
            // Fetch the product from the database
            var product = await _unitOfWork.ProductRepository.FindAsync(id)
                ?? throw new ArgumentNullException("Product does not exist");

            // Update product details
            product.Description = request.Description;
            product.Name = request.Name;
            product.LastUpdateDate = DateTime.UtcNow;

            // Handle image updates
            if (request.Images != null && request.Images.Any())
            {
                // Clear existing images if necessary (optional, based on your use case)
                product.Images.Clear();

                // Upload new images and associate them with the product
                foreach (var imageFile in request.Images)
                {
                    var imageUrl = await _fileStorageService.UploadFileBlobAsync((Microsoft.AspNetCore.Http.IFormFile)imageFile);

                    var imageEntity = new Image
                    {
                        Url = imageUrl,
                        Product = product // Associate image with the product
                    };

                    // Add the new image to the product's Images collection
                    product.Images.Add(imageEntity);
                }
            }

            // Commit the changes to the database
            await _unitOfWork.CommitAsync();

            return new ServiceActionResult(true) { Data = product };
        }




        public async Task<ServiceActionResult> DeleteProduct(Guid id)
        {
            await _unitOfWork.ProductRepository.SoftDeleteByIdAsync(id);
            return new ServiceActionResult();
        }
        public async Task<ServiceActionResult> GetProduct(Guid id)
        {
            var product = (await _unitOfWork.ProductRepository.GetAllAsyncAsQueryable())
                          .Where(a => a.IsDeleted == false && a.Id == id)
                          .Include(a => a.Images)
                          .FirstOrDefault()
                          ?? throw new ArgumentNullException("Product does not exist or has been deleted");

            var returnProduct = _mapper.Map<ProductResponse>(product);

            return new ServiceActionResult(true) { Data = returnProduct };
        }

    }
}
