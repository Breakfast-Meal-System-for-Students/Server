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
using Microsoft.EntityFrameworkCore;
using BMS.BLL.Models.Responses.Package;
using BMS.BLL.Models.Responses.AI;
using BMS.BLL.Utilities;
using BMS.Core.Domains.Enums;

namespace BMS.BLL.Services
{

    public class ProductService : BaseService, IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly IProductAIDetectService _productAIDetectService;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService, IProductAIDetectService productAIDetectService) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
            _productAIDetectService = productAIDetectService;
        }

        public async Task<ServiceActionResult> GetAllProduct(ProductRequest queryParameters)
        {

            IQueryable<Product> ProductQueryable = (await _unitOfWork.ProductRepository.GetAllAsyncAsQueryable()).Where(a => a.IsDeleted == false).Include(a => a.Images);



            if (!string.IsNullOrEmpty(queryParameters.Search))
            {
                ProductQueryable = ProductQueryable.Where(m => m.Description.Contains(queryParameters.Search));
            }

            if (queryParameters.IsOutOfStock != null)
            {
                ProductQueryable = ProductQueryable.Where(m => m.isOutOfStock == queryParameters.IsOutOfStock); 
            }

            if (queryParameters.IsAICanDetect != null)
            {
                ProductQueryable = ProductQueryable.Where(m => m.isAICanDetect == queryParameters.IsAICanDetect);
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
            /*if (productEntity.Inventory.GetValueOrDefault() <= 0)
            {
                productEntity.Inventory = 9999;
            }*/

            ImageAIResponse temp = new ImageAIResponse();
            // Handle image upload and create Image entities
            if (request.Images != null && request.Images.Any())
            {
                foreach (var imageFile in request.Images)
                {
                    /////
                    temp = await _productAIDetectService.DetectImageProductAsync(imageFile, productEntity.Name);
                    if (temp != null && temp.Result.Equals("0"))
                    {
                        return new ServiceActionResult(false)
                        {
                            Detail = temp.Reason,
                            IsSuccess = false,

                        };
                    }
                    ////
                    var imageUrl = await _fileStorageService.UploadFileBlobAsync(imageFile);
                    //Create Image entity and associate with the Product
                    var imageEntity = new Image
                    {
                        Url = imageUrl,
                        Id = productEntity.Id // Associate the image with the product
                    };

                    // Add the image entity to the Product's Images collection
                    productEntity.Images.Add(imageEntity);
                }
                await _unitOfWork.ImageRepository.AddRangeAsync(productEntity.Images);
            }
            productEntity.isAICanDetect = AIDetectStatus.ACCEPTED;
            // Add the Product to the repository
            await _unitOfWork.ProductRepository.AddAsync(productEntity);

            // Commit the transaction
            await _unitOfWork.CommitAsync();

            // Return success result with the created product data
            return new ServiceActionResult(true) { Data = productEntity };
        }

        public async Task<ServiceActionResult> AddProductToStaff(CreateProductRequest request)
        {
            var productEntity = _mapper.Map<Product>(request);
            productEntity.isAICanDetect = AIDetectStatus.PENDING;
            if (request.Images != null && request.Images.Any())
            {
                foreach (var imageFile in request.Images)
                {
                    var imageUrl = await _fileStorageService.UploadFileBlobAsync(imageFile);
                    var imageEntity = new Image
                    {
                        Url = imageUrl,
                        Id = productEntity.Id
                    };

                    productEntity.Images.Add(imageEntity);
                }
                await _unitOfWork.ImageRepository.AddRangeAsync(productEntity.Images);
            }
            await _unitOfWork.ProductRepository.AddAsync(productEntity);

            await _unitOfWork.CommitAsync();

            return new ServiceActionResult(true) { Data = productEntity };
        }

        public async Task<ServiceActionResult> UpdateProduct(Guid id, UpdateProductRequest request)
        {
            // Fetch the product from the database
            var product = await _unitOfWork.ProductRepository.FindAsync(id)
                ?? throw new ArgumentNullException("Product does not exist");

            // Update product details
            product.Price = request.Price;
            product.Description = request.Description;
            product.Name = request.Name;
            product.LastUpdateDate = DateTimeHelper.GetCurrentTime();
            /*if(request.Inventory.GetValueOrDefault() <= 0)
            {
                product.Inventory = 9999;
            } else 
            {
                product.Inventory = request.Inventory;
            }*/
            

            // Handle image updates
            if (request.Images != null && request.Images.Any())
            {
                // Clear existing images if necessary (optional, based on your use case)
                product.Images.Clear();
                var images = (await _unitOfWork.ImageRepository.GetAllAsyncAsQueryable()).Where(x => x.ProductId == product.Id).AsNoTracking().ToList();
                if (images != null && images.Any())
                {
                    await _unitOfWork.ImageRepository.DeleteRangeAsync(images);
                    await _unitOfWork.CommitAsync();
                }
                // Upload new images and associate them with the product
                foreach (var imageFile in request.Images)
                {
                    var imageUrl = await _fileStorageService.UploadFileBlobAsync((Microsoft.AspNetCore.Http.IFormFile)imageFile);

                    var imageEntity = new Image
                    {
                        Url = imageUrl,
                        Id = product.Id // Associate image with the product
                    };

                    // Add the new image to the product's Images collection
                    product.Images.Add(imageEntity);
                }
                if (product.Images != null && product.Images.Any())
                {
                    await _unitOfWork.ImageRepository.AddRangeAsync(product.Images);
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
                          .Include(a => a.Images).Include(a=>a.RegisterCategorys)!.ThenInclude(ab=>ab.Category)
                          .FirstOrDefault()
                          ?? throw new ArgumentNullException("Product does not exist or has been deleted");

            var returnProduct = _mapper.Map<ProductResponse>(product);

            return new ServiceActionResult(true) { Data = returnProduct };
        }
        public async Task<ServiceActionResult> GetAllProductByShopId(Guid id,ProductRequest queryParameters)
        {

            IQueryable<Product> ProductQueryable = (await _unitOfWork.ProductRepository.GetAllAsyncAsQueryable()).Where(a => a.IsDeleted == false && a.ShopId == id && a.isAICanDetect == AIDetectStatus.ACCEPTED).Include(a => a.Images);



            if (!string.IsNullOrEmpty(queryParameters.Search))
            {
                ProductQueryable = ProductQueryable.Where(m => m.Description.Contains(queryParameters.Search));
            }

            if (queryParameters.IsOutOfStock != null)
            {
                ProductQueryable = ProductQueryable.Where(m => m.isOutOfStock == queryParameters.IsOutOfStock);
            }
            /*if (queryParameters.IsAICanDetect != null)
            {
                ProductQueryable = ProductQueryable.Where(m => m.isOutOfStock == queryParameters.IsOutOfStock);
            }*/
            ProductQueryable = queryParameters.IsDesc ? ProductQueryable.OrderByDescending(a => a.CreateDate) : ProductQueryable.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Product, ProductResponse>(_mapper, ProductQueryable, queryParameters.PageSize, queryParameters.PageIndex);



            return new ServiceActionResult() { Data = paginationResult };
        }

        public async Task<int> GetInventoryOfProductInDay(Guid productId, DateTime orderDate)
        {
            var product = await _unitOfWork.ProductRepository.FindAsync(productId);
            var today = orderDate.Date;
            var totalQuantity = (await _unitOfWork.OrderItemRepository.GetAllAsyncAsQueryable())
                .Where(x => x.ProductId == productId && x.CreateDate.Date == today)
                .Sum(x => x.Quantity);
            return product.Inventory.GetValueOrDefault() - totalQuantity;
        }

        public async Task<ServiceActionResult> ChangeOutOfStock(Guid productId)
        {
            var product = (await _unitOfWork.ProductRepository.GetAllAsyncAsQueryable())
                          .Where(a => a.IsDeleted == false && a.Id == productId)
                          .FirstOrDefault()?? throw new ArgumentNullException("Product does not exist or has been deleted");
            product.isOutOfStock = product.isOutOfStock ? false : true;
            return new ServiceActionResult(true)
            {
                Data = product.isOutOfStock,
                Detail = "Change OutOfStock Successfully"
            };
        }

        public async Task<ServiceActionResult> ChangeAICanDetect(Guid productId, AIDetectStatus status)
        {
            var product = (await _unitOfWork.ProductRepository.GetAllAsyncAsQueryable())
                          .Where(a => a.IsDeleted == false && a.Id == productId)
                          .FirstOrDefault() ?? throw new ArgumentNullException("Product does not exist or has been deleted");
            product.isAICanDetect = status;
            return new ServiceActionResult(true)
            {
                Detail = $"Change AIDetect To {status} successfully"
            };
        }
    }
}
