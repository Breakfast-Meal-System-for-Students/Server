using BMS.BLL.Models.Requests.Package;
using BMS.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.BLL.Models.Requests.Basic;

namespace BMS.BLL.Services.IServices
{
 
    public interface IPackageService
    {
        Task<ServiceActionResult> GetAllPackage(PackageRequest queryParameters);
        Task<ServiceActionResult> AddPackage(CreatePackageRequest request);

        Task<ServiceActionResult> UpdatePackage(Guid id, UpdatePackageRequest request);
        Task<ServiceActionResult> DeletePackage(Guid id);
        Task<ServiceActionResult> GetPackage(Guid id);
        Task<ServiceActionResult> GetPackageForShopInUse(Guid shopId, PackageRequest request);
        Task<ServiceActionResult> GetPackageForHistoryBuyingByShop(Guid shopId, PackageRequest request);
        Task<ServiceActionResult> BuyPackageByShop(Guid shopId, Guid packageId);

        Task<ServiceActionResult> GetRevenueForBuyPackage(GetRevenueForBuyPackageRequest request);
        Task<ServiceActionResult> GetAmountAndRevenueOfEachPackage(GetAmountAndRevenueOfEachPackageRequest request);
    }
}
