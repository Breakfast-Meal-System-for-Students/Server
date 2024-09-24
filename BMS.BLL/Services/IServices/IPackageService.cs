using BMS.BLL.Models.Requests.Package;
using BMS.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.IServices
{
 
    public interface IPackageService
    {
        Task<ServiceActionResult> GetAllPackage(PackageRequest queryParameters);
        Task<ServiceActionResult> AddPackage(CreatePackageRequest request);

        Task<ServiceActionResult> UpdatePackage(Guid id, UpdatePackageRequest request);
        Task<ServiceActionResult> DeletePackage(Guid id);
        Task<ServiceActionResult> GetPackage(Guid id);

    }
}
