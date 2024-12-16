using BMS.BLL.Models.Requests.Category;
using BMS.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.BLL.Models.Requests.Coupon;

namespace BMS.BLL.Services.IServices
{

    public interface ICouponService
    {
        Task<ServiceActionResult> GetAllCoupon(CouponRequest queryParameters);
        Task<ServiceActionResult> AddCoupon(CreateCouponRequest request);

        Task<ServiceActionResult> UpdateCoupon(Guid id, UpdateCouponRequest request);
        Task<ServiceActionResult> DeleteCoupon(Guid id);
        Task<ServiceActionResult> GetCoupon(Guid id);
        Task<ServiceActionResult> GetAllCouponForShop(Guid Shopid, CouponRequest queryParameters);
        Task<ServiceActionResult> GetAllCouponForShopInWeb(Guid Shopid, CouponRequest queryParameters);
        Task<ServiceActionResult> GetDiscountAmount(Guid voucherId, float amount);

    }
}
