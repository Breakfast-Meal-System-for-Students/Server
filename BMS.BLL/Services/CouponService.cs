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
using Microsoft.AspNetCore.Identity;
using BMS.BLL.Utilities;
using Azure.Core;

namespace BMS.BLL.Services
{
  
    public class CouponService : BaseService, ICouponService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly UserManager<User> _userManager;
        public CouponService(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService, UserManager<User> userManager) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
            _userManager = userManager;
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
            if(request.StartDate < DateTimeHelper.GetCurrentTime()) request.StartDate = DateTimeHelper.GetCurrentTime();
            if(request.EndDate < DateTimeHelper.GetCurrentTime()) request.EndDate = DateTimeHelper.GetCurrentTime();
            if(request.StartDate > request.EndDate)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "The EndDate must be later than the StartDate"
                };
            }
            if(request.MinPrice < 0)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "The MinPrice must be >= 0"
                };
            }

            if (request.MaxDiscount <= 0)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "The MaxDiscount must be > 0"
                };
            }

            if (request.isPercentDiscount)
            {
                if (request.PercentDiscount <= 0)
                {
                    return new ServiceActionResult(false)
                    {
                        Detail = "The PercentDiscount must be > 0"
                    };
                }
            }
            else
            {
                if (request.MinDiscount <= 0)
                {
                    return new ServiceActionResult(false)
                    {
                        Detail = "The MinDiscount must be > 0"
                    };
                }
            }

            var shop = await _unitOfWork.ShopRepository.FindAsync(request.ShopId);
            if (shop == null) 
            {
                return new ServiceActionResult(false)
                {
                    Detail = "The ShopId is invalid"
                };
            }

            var CouponEntity = _mapper.Map<Coupon>(request);
      
            await _unitOfWork.CouponRepository.AddAsync(CouponEntity);

            return new ServiceActionResult(true) { Data = CouponEntity };
        }

        public async Task<ServiceActionResult> UpdateCoupon(Guid id, UpdateCouponRequest request)
        {
            if (request.StartDate < DateTimeHelper.GetCurrentTime()) request.StartDate = DateTimeHelper.GetCurrentTime();
            if (request.EndDate < DateTimeHelper.GetCurrentTime()) request.EndDate = DateTimeHelper.GetCurrentTime();
            if (request.StartDate > request.EndDate)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "The EndDate must be later than the StartDate"
                };
            }
            if (request.MinPrice < 0)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "The MinPrice must be >= 0"
                };
            }

            if (request.MaxDiscount <= 0)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "The MaxDiscount must be > 0"
                };
            }

            if (request.isPercentDiscount)
            {
                if (request.PercentDiscount <= 0)
                {
                    return new ServiceActionResult(false)
                    {
                        Detail = "The MaxDiscount must be > 0"
                    };
                }
            }
            else
            {
                if (request.MinDiscount <= 0)
                {
                    return new ServiceActionResult(false)
                    {
                        Detail = "The MinDiscount must be > 0"
                    };
                }
            }
            var Coupon = await _unitOfWork.CouponRepository.FindAsync(id) ?? throw new ArgumentNullException("Coupon is not exist");
     

            Coupon.LastUpdateDate = DateTimeHelper.GetCurrentTime();
            Coupon.Name = request.Name;
            Coupon.MaxDiscount = request.MaxDiscount;
            Coupon.StartDate = request.StartDate;
            Coupon.EndDate = request.EndDate;
            Coupon.PercentDiscount = request.PercentDiscount;
            Coupon.MinDiscount = request.MinDiscount;
            Coupon.MinPrice = request.MinPrice;
            Coupon.isPercentDiscount = request.isPercentDiscount;
            await _unitOfWork.CommitAsync();

            return new ServiceActionResult(true) { Data = Coupon, Detail = "Update Coupon succeesfully" };
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
        public async Task<ServiceActionResult> GetAllCouponForShop(Guid Shopid, CouponRequest queryParameters)
        {
            IQueryable<Coupon> CouponQueryable = (await _unitOfWork.CouponRepository.GetAllAsyncAsQueryable()).Where(a => a.IsDeleted == false && a.ShopId == Shopid && a.EndDate > DateTimeHelper.GetCurrentTime() && a.StartDate < DateTimeHelper.GetCurrentTime());

            if (!string.IsNullOrEmpty(queryParameters.Search))
            {
                CouponQueryable = CouponQueryable.Where(m => m.Name.Contains(queryParameters.Search));
            }

            CouponQueryable = queryParameters.IsDesc ? CouponQueryable.OrderByDescending(a => a.CreateDate) : CouponQueryable.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Coupon, CouponResponse>(_mapper, CouponQueryable, queryParameters.PageSize, queryParameters.PageIndex);



            return new ServiceActionResult() { Data = paginationResult };
        }
        public async Task<ServiceActionResult> GetAllCouponForShopInWeb(Guid Shopid, CouponRequest queryParameters)
        {
            IQueryable<Coupon> CouponQueryable = (await _unitOfWork.CouponRepository.GetAllAsyncAsQueryable()).Where(a => a.IsDeleted == false && a.ShopId == Shopid);

            if (!string.IsNullOrEmpty(queryParameters.Search))
            {
                CouponQueryable = CouponQueryable.Where(m => m.Name.Contains(queryParameters.Search));
            }

            CouponQueryable = queryParameters.IsDesc ? CouponQueryable.OrderByDescending(a => a.CreateDate) : CouponQueryable.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Coupon, CouponResponse>(_mapper, CouponQueryable, queryParameters.PageSize, queryParameters.PageIndex);



            return new ServiceActionResult() { Data = paginationResult };
        }
        public async Task<ServiceActionResult> GetAllCouponForUser(Guid UserId,Guid Shopid, CouponRequest queryParameters)
        {

            IQueryable<Coupon> CouponQueryable = (await _unitOfWork.CouponRepository.GetAllAsyncAsQueryable()).Where(a => a.IsDeleted == false && a.ShopId == Shopid);



            if (!string.IsNullOrEmpty(queryParameters.Search))
            {
                CouponQueryable = CouponQueryable.Where(m => m.Name.Contains(queryParameters.Search));
            }

            CouponQueryable = queryParameters.IsDesc ? CouponQueryable.OrderByDescending(a => a.CreateDate) : CouponQueryable.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Coupon, CouponResponse>(_mapper, CouponQueryable, queryParameters.PageSize, queryParameters.PageIndex);



            return new ServiceActionResult() { Data = paginationResult };
        }

        public async Task<ServiceActionResult> GetDiscountAmount(Guid voucherId, float amount)
        {
            var coupon = await _unitOfWork.CouponRepository.FindAsync(voucherId);
            if (coupon == null || coupon.IsDeleted)
            {
                return new ServiceActionResult(false) { Detail = "Coupon does not exist or is deleted" };
            }
            /*if (coupon.ShopId != order.ShopId)
            {
                return new ServiceActionResult(false) { Detail = "Coupon is not used in this shop" };
            }
            if (coupon.StartDate >= order.CreateDate)
            {
                return new ServiceActionResult(false) { Detail = "The Date of Coupon is not yet start" };
            }
            else if (coupon.EndDate <= order.CreateDate)
            {
                return new ServiceActionResult(false) { Detail = "The Date of Coupon is Finish" };
            }*/
            else if (amount < coupon.MinPrice)
            {
                return new ServiceActionResult(false) { Detail = "Total Price of Order is not enough to use this voucher" };
            }

            var discount = coupon.isPercentDiscount
                ? Math.Min(amount * coupon.PercentDiscount, coupon.MaxDiscount)
                : coupon.MinDiscount;
            if (discount > 0)
            {
                if (amount - discount <= 0)
                {
                    return new ServiceActionResult(false) { Detail = "Total Price of Order is < 0" };
                }
                return new ServiceActionResult(true)
                {
                    Data = amount - discount
                };
            }
            return new ServiceActionResult(true)
            {
                Data = amount
            };
        }
    }
}
