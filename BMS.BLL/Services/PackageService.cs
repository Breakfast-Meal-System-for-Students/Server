using AutoMapper;
using BMS.BLL.Models.Requests.Package;
using BMS.BLL.Models.Responses.Package;
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
using BMS.BLL.Models.Requests.Basic;
using Microsoft.EntityFrameworkCore;

namespace BMS.BLL.Services
{

    public class PackageService : BaseService, IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PackageService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceActionResult> GetAllPackage(PackageRequest queryParameters)
        {

            IQueryable<Package> PackageQueryable = (await _unitOfWork.PackageRepository.GetAllAsyncAsQueryable()).Where(a => a.IsDeleted == false);



            if (!string.IsNullOrEmpty(queryParameters.Search))
            {
                PackageQueryable = PackageQueryable.Where(m => m.Name.Contains(queryParameters.Search));
            }

            PackageQueryable = queryParameters.IsDesc ? PackageQueryable.OrderByDescending(a => a.CreateDate) : PackageQueryable.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Package, PackageResponse>(_mapper, PackageQueryable, queryParameters.PageSize, queryParameters.PageIndex);



            return new ServiceActionResult() { Data = paginationResult };
        }
        public async Task<ServiceActionResult> AddPackage(CreatePackageRequest request)
        {
            if (request.Name.Trim() == "") 
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Name is not Empty"
                };
            }
            if (request.Description.Trim() == "")
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Description is not Empty"
                };
            }
            if (request.Name.Trim() == "")
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Name is not Empty"
                };
            }
            if (request.Price <= 50000)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Price is must be more than 50000"
                };
            }
            if (request.Duration <= 1)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Duration is must be more than 1"
                };
            }

            var PackageEntity = _mapper.Map<Package>(request);

            await _unitOfWork.PackageRepository.AddAsync(PackageEntity);
            // do something add feedback
            await _unitOfWork.CommitAsync();

            return new ServiceActionResult(true) { Data = PackageEntity };
        }

        public async Task<ServiceActionResult> UpdatePackage(Guid id, UpdatePackageRequest request)
        {
            if (request.Name.Trim() == "")
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Name is not Empty"
                };
            }
            if (request.Description.Trim() == "")
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Description is not Empty"
                };
            }
            if (request.Name.Trim() == "")
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Name is not Empty"
                };
            }
            if (request.Price <= 50000)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Price is must be more than 50000"
                };
            }
            if (request.Duration <= 1)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Duration is must be more than 1"
                };
            }

            var Package = await _unitOfWork.PackageRepository.FindAsync(id) ?? throw new ArgumentNullException("Package is not exist");


            Package.LastUpdateDate = DateTime.UtcNow;
            Package.Name = request.Name;
            Package.Description = request.Description;
            Package.Price   = request.Price;
            Package.Duration = request.Duration;
            await _unitOfWork.CommitAsync();

            return new ServiceActionResult(true) { Data = Package };
        }


        public async Task<ServiceActionResult> GetPackage(Guid id)
        {
            var Package = await _unitOfWork.PackageRepository
         .FindAsync(c => c.Id == id && c.IsDeleted == false)
         ?? throw new ArgumentNullException("Package does not exist or has been deleted");
            var returnPackage = _mapper.Map<PackageResponse>(Package);

            return new ServiceActionResult(true) { Data = returnPackage };
        }
        public async Task<ServiceActionResult> DeletePackage(Guid id)
        {
            await _unitOfWork.PackageRepository.SoftDeleteByIdAsync(id);
            return new ServiceActionResult(true)
            {
                Detail = "Delete Successfully"
            };
        }

        public async Task<ServiceActionResult> GetPackageForShopInUse(Guid shopId, PackageRequest request)
        {
            var shop = await _unitOfWork.ShopRepository.FindAsync(shopId);
            if (shop == null || shop.IsDeleted == true)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Shop is not valid or delete"
                };
            }
            IQueryable<Package> packages = (await _unitOfWork.PackageRepository.GetAllAsyncAsQueryable()).Include(a => a.Package_Shop)
                            .Where(x => x.Package_Shop.Any(y => y.ShopId == shopId && y.CreateDate.AddDays(x.Duration) > DateTime.UtcNow) && x.IsDeleted == false);
            if (!string.IsNullOrEmpty(request.Search))
            {
                packages = packages.Where(m => m.Name.Contains(request.Search));
            }

            packages = request.IsDesc ? packages.OrderByDescending(a => a.CreateDate) : packages.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Package, PackageResponse>(_mapper, packages, request.PageSize, request.PageIndex);
            return new ServiceActionResult(true) 
            { 
                Data = paginationResult 
            };
        }

        public async Task<ServiceActionResult> GetPackageForHistoryBuyingByShop(Guid shopId, PackageRequest request)
        {
            var shop = await _unitOfWork.ShopRepository.FindAsync(shopId);
            if (shop == null || shop.IsDeleted == true)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Shop is not valid or delete"
                };
            }
            IQueryable<Package> packages = (await _unitOfWork.PackageRepository.GetAllAsyncAsQueryable()).Include(a => a.Package_Shop)
                            .Where(x => x.Package_Shop.Any(y => y.ShopId == shopId && y.CreateDate.AddDays(x.Duration) < DateTime.UtcNow));
            if (!string.IsNullOrEmpty(request.Search))
            {
                packages = packages.Where(m => m.Name.Contains(request.Search));
            }

            packages = request.IsDesc ? packages.OrderByDescending(a => a.CreateDate) : packages.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Package, PackageResponse>(_mapper, packages, request.PageSize, request.PageIndex);
            return new ServiceActionResult(true)
            {
                Data = paginationResult
            };
        }

        public async Task<ServiceActionResult> BuyPackageByShop(Guid shopId, Guid packageId)
        {
            var shop = await _unitOfWork.ShopRepository.FindAsync(shopId);
            if (shop == null || shop.IsDeleted == true)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Shop is not valid or delete"
                };
            }
            var package = await _unitOfWork.PackageRepository.FindAsync(packageId);
            if (package == null || package.IsDeleted == true)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Package is not valid or delete"
                };
            }
            var packageDB = (await _unitOfWork.Package_ShopRepository.GetAllAsyncAsQueryable()).Include(x => x.Package).Where(x => x.ShopId == shopId && x.PackageId == packageId && x.CreateDate.AddDays(x.Package.Duration) > DateTime.UtcNow).ToList();
            if(packageDB != null && packageDB.Any())
            {
                return new ServiceActionResult(false)
                {
                    Detail = "The Package is being used in Shop. You can not buy the same Package."
                };
            }
            Package_Shop package_Shop = new Package_Shop()
            {
                ShopId = shopId,
                PackageId = packageId,
            };
            await _unitOfWork.Package_ShopRepository.AddAsync(package_Shop);
            return new ServiceActionResult(true)
            {
                Data = package_Shop,
                Detail = "Buy Package succeessfully"
            };
        }
    }
}
