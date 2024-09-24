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


            var PackageEntity = _mapper.Map<Package>(request);

            await _unitOfWork.PackageRepository.AddAsync(PackageEntity);
            // do something add feedback
            await _unitOfWork.CommitAsync();

            return new ServiceActionResult(true) { Data = PackageEntity };
        }

        public async Task<ServiceActionResult> UpdatePackage(Guid id, UpdatePackageRequest request)
        {


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
            return new ServiceActionResult();
        }

    }
}
