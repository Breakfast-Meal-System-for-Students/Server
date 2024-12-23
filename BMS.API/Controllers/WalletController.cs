using BMS.API.Controllers.Base;
using BMS.BLL.Models;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services;
using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BMS.BLL.Models.Requests.Admin;
using Microsoft.AspNetCore.Authorization;
using BMS.BLL.Models.Requests;
using BMS.BLL.Models.Requests.Basic;

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
        [HttpGet("GetWalletByUserId")]
        [Authorize]
        public async Task<IActionResult> GetWalletByUserId()
        {
            return await ExecuteServiceLogic(
                 async () => await _walletService.GetWalletByUserId(_userClaims.UserId).ConfigureAwait(false)
             ).ConfigureAwait(false);
        }

        [HttpPut("UpdateBalance")]
        [Authorize]
        public async Task<IActionResult> UpdateBalance([FromForm] UpdateBalanceRequest request)
        {
            return await ExecuteServiceLogic(
                 async () => await _walletService.UpdateBalance(_userClaims.UserId, request.Status, request.Amount, request.OrderId).ConfigureAwait(false)
             ).ConfigureAwait(false);
        }

        [HttpGet("GetAllTransactionOfUserWallet")]
        [Authorize]
        public async Task<IActionResult> GetAllTransactionOfUserWallet([FromQuery] PagingRequest request)
        {
            return await ExecuteServiceLogic(
                 async () => await _walletService.GetAllTransactionOfUserWallet(_userClaims.UserId, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
    }
}
