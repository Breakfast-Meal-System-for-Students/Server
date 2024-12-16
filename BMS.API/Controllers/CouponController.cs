using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.Coupon;
using BMS.BLL.Services;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
  
    public class CouponController : BaseApiController
    {
        private readonly ICouponService _couponService;

 

        public CouponController(ICouponService CouponService)
        {
            _couponService = CouponService;

        

            _baseService = (BaseService)_couponService;
        }


        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteCoupon(Guid id)
        {

            return await ExecuteServiceLogic(
                               async () => await _couponService.DeleteCoupon(id).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponRequest Coupon)
        {
            return await ExecuteServiceLogic(
                               async () => await _couponService.AddCoupon(Coupon).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpGet("get-all-coupon")]
        public async Task<IActionResult> GetAllCoupon([FromQuery] CouponRequest pagingRequest)
        {
            return await ExecuteServiceLogic(
                               async () => await _couponService.GetAllCoupon(pagingRequest).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpGet("get-all-coupon-for-shop")]
        public async Task<IActionResult> GetAllCouponForShop(Guid shopId,[FromQuery] CouponRequest pagingRequest)
        {
            return await ExecuteServiceLogic(
                               async () => await _couponService.GetAllCouponForShop(shopId,pagingRequest).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpGet("GetAllCouponForShopInWeb")]
        public async Task<IActionResult> GetAllCouponForShopInWeb(Guid shopId, [FromQuery] CouponRequest pagingRequest)
        {
            return await ExecuteServiceLogic(
                               async () => await _couponService.GetAllCouponForShopInWeb(shopId, pagingRequest).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCoupon(Guid Id, [FromBody] UpdateCouponRequest Coupon)
        {
            return await ExecuteServiceLogic(
                               async () => await _couponService.UpdateCoupon(Id, Coupon).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCoupon(Guid id)
        {
            return await ExecuteServiceLogic(
                async () => await _couponService.GetCoupon(id).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
        [HttpGet("GetAmountOfOrderWhenUseVoucher")]
        [Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> GetDiscountAmount(Guid voucherId, float amount)
        {
            return await ExecuteServiceLogic(
                async () => await _couponService.GetDiscountAmount(voucherId, amount).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
    }
}
