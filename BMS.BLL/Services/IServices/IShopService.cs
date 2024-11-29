using BMS.BLL.Models.Requests.Shop;
using BMS.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.BLL.Models.Requests.Package;
using BMS.Core.Domains.Entities;

namespace BMS.BLL.Services.IServices
{
  
    public interface IShopService
    {

        Task<ServiceActionResult> UpdateShop(Guid id, UpdateShopRequest request);
        Task<ServiceActionResult> UpdateShopByStaff(Guid id, UpdateShopRequestByStaff request);
        Task<ServiceActionResult> DeleteShop(Guid id);
        Task<ServiceActionResult> GetShop(Guid id);
        Task<ServiceActionResult> GetAllShop(ShopRequest queryParameters);
        Task<ServiceActionResult> GetAllShopForMobile(ShopRequest request);
        Task<List<Shop>> GetAllShopToRevenue(DateTime startDate, DateTime endDate);
        Task<ServiceActionResult> CountNewShop(TotalShopRequest request);
    }
}
