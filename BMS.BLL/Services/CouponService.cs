using AutoMapper;
using BMS.BLL.Models.Requests.Coupon;
using BMS.BLL.Models.Responses.Coupon;
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
using BMS.Core.Domains.Enums;
using BMS.BLL.Models.Responses.Shop;

namespace BMS.BLL.Services
{
  
    public class CouponService : BaseService, ICouponService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        public CouponService(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        public async Task<ServiceActionResult> GetAllCoupon(CouponRequest queryParameters)
        {

            IQueryable<Coupon> CouponQueryable = (await _unitOfWork.CouponRepository.GetAllAsyncAsQueryable()).Where(a => a.IsDeleted == false);

    

            if (!string.IsNullOrEmpty(queryParameters.Search))
            {
                CouponQueryable = CouponQueryable.Where(m => m.Name.Contains(queryParameters.Search));
            }

            CouponQueryable = queryParameters.IsDesc ? CouponQueryable.OrderByDescending(a => a.CreateDate) : CouponQueryable.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Coupon, CouponResponse>(_mapper, CouponQueryable, queryParameters.PageSize, queryParameters.PageIndex);



            return new ServiceActionResult() { Data = paginationResult };
        }
        public async Task<ServiceActionResult> AddCoupon(CreateCouponRequest request)
        {


            var CouponEntity = _mapper.Map<Coupon>(request);
      
            await _unitOfWork.CouponRepository.AddAsync(CouponEntity);
            // do something add feedback
            await _unitOfWork.CommitAsync();

            return new ServiceActionResult(true) { Data = CouponEntity };
        }

        public async Task<ServiceActionResult> UpdateCoupon(Guid id, UpdateCouponRequest request)
        {


            var Coupon = await _unitOfWork.CouponRepository.FindAsync(id) ?? throw new ArgumentNullException("Coupon is not exist");
     

            Coupon.LastUpdateDate = DateTime.UtcNow;
            Coupon.Name = request.Name;
            Coupon.MaxDiscount = request.MaxDiscount;
            Coupon.StartDate = request.StartDate;
            Coupon.EndDate = request.EndDate;
            Coupon.PercentDiscount = request.PercentDiscount;
            Coupon.MinDiscount = request.MinDiscount;
            Coupon.MinPrice = request.MinPrice;
            await _unitOfWork.CommitAsync();

            return new ServiceActionResult(true) { Data = Coupon };
        }


        public async Task<ServiceActionResult> GetCoupon(Guid id)
        {
            var coupon = await _unitOfWork.CouponRepository
         .FindAsync(c => c.Id == id && c.IsDeleted == false)
         ?? throw new ArgumentNullException("Coupon does not exist or has been deleted");
            var returnCoupon = _mapper.Map<CouponResponse>(coupon);

            return new ServiceActionResult(true) { Data = returnCoupon };
        }
        public async Task<ServiceActionResult> DeleteCoupon(Guid id)
        {
            await _unitOfWork.CouponRepository.SoftDeleteByIdAsync(id);
            return new ServiceActionResult();
        }

    }
}
