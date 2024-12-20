using BMS.API.Controllers.Base;
using BMS.BLL.Models;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services;
using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BMS.BLL.Models.Requests.Admin;

namespace BMS.API.Controllers
{
    public class WalletController : BaseApiController
    {
        private readonly IWalletService _walletService;
        private readonly IUserClaimsService _userClaimsService;
        private UserClaims _userClaims;
        public WalletController(IWalletService walletService, IUserClaimsService userClaimsService)
        {
            _walletService = walletService;
            _baseService = (BaseService)walletService;
            _userClaimsService = userClaimsService;
            _userClaims = userClaimsService.GetUserClaims();
        }
        [HttpGet("GetWalletById")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> GetWalletById([FromQuery] SearchOrderRequest request)
        {
            return Ok();
        }
    }
}
