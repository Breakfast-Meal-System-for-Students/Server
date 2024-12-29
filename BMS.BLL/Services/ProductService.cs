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
                ProductQueryable = ProductQueryable.Where(m => m.Description.Contains(queryParameters.Search) || m.Name.Contains(queryParameters.Search));
            }

            if (queryParameters.IsOutOfStock != null)
            {
                ProductQueryable = ProductQueryable.Where(m => m.isOutOfStock == queryParameters.IsOutOfStock); 
            }

            if (queryParameters.IsAICanDetect != null)
            {
                ProductQueryable = ProductQueryable.Where(m => m.isAICanDetect == queryParameters.IsAICanDetect);
            }

            if (queryParameters.IsCombo != null)
            {
                ProductQueryable = ProductQueryable.Where(m => m.IsCombo == queryParameters.IsCombo);
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

            productEntity.isAICanDetect = AIDetectStatus.ACCEPTED;
            await _unitOfWork.ProductRepository.AddAsync(productEntity);

            ImageAIResponse temp = new ImageAIResponse();
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
                        ProductId = productEntity.Id // Associate the image with the product
                    };

                    // Add the image entity to the Product's Images collection
                    productEntity.Images.Add(imageEntity);
                }
                await _unitOfWork.ImageRepository.AddRangeAsync(productEntity.Images);
            }

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
                        ProductId = product.Id 
                    };

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
            var numberOfOrders = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Include(a => a.OrderItems)
                    .Where(x => x.OrderItems.Any(y => y.ProductId == id) && (x.Status.Equals(OrderStatus.ORDERED.ToString()) || x.Status.Equals(OrderStatus.CHECKING.ToString()))
                    && x.OrderDate > DateTimeHelper.GetCurrentTime().AddMinutes(-30))
                    .Select(x => new { x.Id, x.CustomerId }).ToList();

            if (numberOfOrders != null && numberOfOrders.Count != 0)
            {
                var listOrderId = numberOfOrders.Select(o => o.Id).Distinct().ToList();
                var listCustomerId = numberOfOrders.Select(o => o.CustomerId).Distinct().ToList();
                return new ServiceActionResult(false)
                {
                    Data = new
                    {
                        listOrderId = listOrderId,
                        listCustomerId = listCustomerId,
                        count = listOrderId.Count
                    },
                    Detail = $"You are having {numberOfOrders.Count} in {OrderStatus.ORDERED.ToString()} or {OrderStatus.CHECKING.ToString()} now. We will cancel them. The rest orders in another status can not be cancel. Are you sure about changing this product to out of stock ?"
                };
            }
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
                ProductQueryable = ProductQueryable.Where(m => m.Description.Contains(queryParameters.Search) || m.Name.Contains(queryParameters.Search));
            }

            if (queryParameters.IsOutOfStock != null)
            {
                ProductQueryable = ProductQueryable.Where(m => m.isOutOfStock == queryParameters.IsOutOfStock);
            }
            /*if (queryParameters.IsAICanDetect != null)
            {
                ProductQueryable = ProductQueryable.Where(m => m.isOutOfStock == queryParameters.IsOutOfStock);
            }*/

            if (queryParameters.IsCombo != null)
            {
                ProductQueryable = ProductQueryable.Where(m => m.IsCombo == queryParameters.IsCombo);
            }
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
                          .FirstOrDefault();
            if(product == null)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Product does not exist or has been deleted"
                };
            }
            if (product.isOutOfStock == false)
            {
                var numberOfOrders = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Include(a => a.OrderItems)
                    .Where(x => x.OrderItems.Any(y => y.ProductId == productId) && (x.Status.Equals(OrderStatus.ORDERED.ToString()) || x.Status.Equals(OrderStatus.CHECKING.ToString()))
                    && x.OrderDate > DateTimeHelper.GetCurrentTime().AddMinutes(-30))
                    .Select(x => new { x.Id, x.CustomerId }).ToList();

                if (numberOfOrders != null && numberOfOrders.Count != 0)
                {
                    var listOrderId = numberOfOrders.Select(o => o.Id).Distinct().ToList();
                    var listCustomerId = numberOfOrders.Select(o => o.CustomerId).Distinct().ToList();
                    return new ServiceActionResult(false)
                    {
                        Data = new
                        {
                            listOrderId = listOrderId,
                            listCustomerId = listCustomerId,
                            count = listOrderId.Count
                        },
                        Detail = $"You are having {numberOfOrders.Count} in {OrderStatus.ORDERED.ToString()} or {OrderStatus.CHECKING.ToString()} now. We will cancel them. The rest orders in another status can not be cancel. Are you sure about changing this product to out of stock ?"
                    };
                }
            }
            product.isOutOfStock = product.isOutOfStock ? false : true;
            await _unitOfWork.ProductRepository.UpdateAsync(product);
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

        public async Task<ServiceActionResult> GetProductBestSellerInShop(Guid shopId, ProductBestSellerRequest request)
        {
            IQueryable<OrderItem> productQuery = (await _unitOfWork.OrderItemRepository.GetAllAsyncAsQueryable())
                                       .Include(b => b.Product).Include(a => a.Order).Where(x => x.Order.ShopId == shopId);
            if (request.Year != 0)
            {
                if (request.Month != 0)
                {
                    if (request.Day != 0)
                    {
                        productQuery = productQuery.Where(x => x.CreateDate.Year == request.Year && x.CreateDate.Month == request.Month && x.CreateDate.Day == request.Day);
                    }
                    else
                    {
                        productQuery = productQuery.Where(x => x.CreateDate.Year == request.Year && x.CreateDate.Month == request.Month);
                    }
                }
                else
                {
                    productQuery = productQuery.Where(x => x.CreateDate.Year == request.Year);
                }
            }
            

            var productBestSeller = productQuery
                .GroupBy(product => product.ProductId)
                .Select(group => new
                {
                    ProductId = group.Key,
                    ProductName = group.FirstOrDefault().Product.Name,
                    ProductImage = group.FirstOrDefault().Product.Images,
                    TotalSold = group
                        .Sum(orderitem => orderitem.Quantity)
                })
                .OrderByDescending(x => x.TotalSold);

            var paginationResult = PaginationHelper.BuildPaginatedResult(productBestSeller, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };
        }

        public async Task<ServiceActionResult> GetProductToPreparingForShopInTime(Guid shopId, GetProductToPreparingRequest request)
        {
            IQueryable<OrderItem> productQuery = (await _unitOfWork.OrderItemRepository.GetAllAsyncAsQueryable())
                                       .Include(b => b.Product).Include(a => a.Order).Where(x => x.Order.ShopId == shopId && x.Order.OrderDate >= request.DateFrom && x.Order.OrderDate <= request.DateTo);

            if (request.OrderStatus != 0)
            {
                productQuery = productQuery.Where(m => m.Order.Status.Equals(request.OrderStatus.ToString()));
            }

            var productSell = productQuery
                .GroupBy(product => new { product.ProductId, product.Note })
                .Select(group => new
                {
                    ProductId = group.Key.ProductId,
                    Note = group.Key.Note,
                    ProductName = group.FirstOrDefault().Product.Name,
                    ProductImage = group.FirstOrDefault().Product.Images,
                    TotalSell = group.Sum(orderitem => orderitem.Quantity)
                })
                .OrderByDescending(x => x.TotalSell);
            var paginationResult = PaginationHelper.BuildPaginatedResult(productSell, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };
        }
    }
}
