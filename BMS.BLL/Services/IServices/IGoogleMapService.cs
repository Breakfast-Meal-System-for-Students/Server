using BMS.BLL.Models.Requests.Map;
using BMS.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.IServices
{
    public interface IGoogleMapService
    {
        Task<ServiceActionResult> ComputeRoutes(RouteRequest request);
        Task<ServiceActionResult> GetShopsByShortestTravelTime(string add1, string add2);
        Task<ServiceActionResult> GetShopsByShortestTravelTime2(string add1, string add2);
        Task<ServiceActionResult> GetShopsByShortestTravelTime3(string add1, string add2, string search);
    }
}
