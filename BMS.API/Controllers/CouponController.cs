﻿using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.Coupon;
using BMS.BLL.Services;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
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
        [Authorize]
        public async Task<IActionResult> DeleteCoupon(Guid id)
        {

            return await ExecuteServiceLogic(
                               async () => await _couponService.DeleteCoupon(id).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponRequest Coupon)
        {
            return await ExecuteServiceLogic(
                               async () => await _couponService.AddCoupon(Coupon).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpGet("get-all-coupon")]
        [Authorize]
        public async Task<IActionResult> GetAllCoupon([FromQuery] CouponRequest pagingRequest)
        {
            return await ExecuteServiceLogic(
                               async () => await _couponService.GetAllCoupon(pagingRequest).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpGet("get-all-coupon-for-shop")]
        [Authorize]
        public async Task<IActionResult> GetAllCouponForShop(Guid shopId,[FromQuery] CouponRequest pagingRequest)
        {
            return await ExecuteServiceLogic(
                               async () => await _couponService.GetAllCouponForShop(shopId,pagingRequest).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCoupon(Guid Id, [FromBody] UpdateCouponRequest Coupon)
        {
            return await ExecuteServiceLogic(
                               async () => await _couponService.UpdateCoupon(Id, Coupon).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetCoupon(Guid id)
        {
            return await ExecuteServiceLogic(
                async () => await _couponService.GetCoupon(id).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
    }
}
