using BMS.BLL.Models.Requests.Shop;
using BMS.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.BLL.Models.Requests.Package;

namespace BMS.BLL.Services.IServices
{
  
    public interface IShopService
    {

        Task<ServiceActionResult> UpdateShop(Guid id, UpdateShopRequest request);
        Task<ServiceActionResult> DeleteShop(Guid id);
        Task<ServiceActionResult> GetShop(Guid id);
        Task<ServiceActionResult> GetAllShop(ShopRequest queryParameters);
    }
}
