using BMS.BLL.Models.Requests.Map;
using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
  
    public class GoogleMapController : ControllerBase
    {
        private readonly IGoogleMapService _googleMapService;

        public GoogleMapController(IGoogleMapService googleMapService)
        {
            _googleMapService = googleMapService;
        }

        [HttpPost]
        [Route("computeRoutes")]
        public async Task<IActionResult> Route([FromBody] RouteRequest request)
        {
            var data = await _googleMapService.ComputeRoutes(request);
            return Ok(data);
        }
        [HttpGet]
        [Route("GetShop")]
        public async Task<IActionResult> GetShop(string add1, string add2, string search)
        {
            var data = await _googleMapService.GetShopsByShortestTravelTime3(add1,add2,search);
            return Ok(data);
        }
    }
}
