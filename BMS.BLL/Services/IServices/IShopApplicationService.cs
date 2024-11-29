using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.IServices
{

    public interface IShopApplicationService
    {
        Task<ServiceActionResult> CreateShopApplication(CreateShopApplicationRequest applicationRequest);
        Task<ServiceActionResult> GetAllApplications(ShopApplicationRequest request);
        Task<ServiceActionResult> GetApplication(Guid id);
        Task<ServiceActionResult> ReviewApplication(Guid id, string status, string message);
    }
}
