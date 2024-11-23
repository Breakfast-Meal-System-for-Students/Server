using BMS.API.Controllers.Base;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Models.Requests.Transaction;
using BMS.BLL.Services;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Constants;
using BMS.Core.Domains.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
    public class TransactionController : BaseApiController
    {
        private readonly ITransactionService _transactionService;
        private readonly IUserClaimsService _userClaimsService;
        private UserClaims _userClaims;
        public TransactionController(ITransactionService transactionService, IUserClaimsService userClaimsService)
        {
            _transactionService = transactionService;
            _baseService = (BaseService)transactionService;
            _userClaimsService = userClaimsService;
            _userClaims = userClaimsService.GetUserClaims();
        }

        [HttpGet("GetListTransactions")]
        [Authorize(Roles = UserRoleConstants.ADMIN + "," + UserRoleConstants.STAFF)]
        public async Task<IActionResult> GetListTransactions([FromQuery]SearchTransactionRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _transactionService.GetListTracsactions(request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetTransactionById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetTransactionById(Guid id)
        {
            return await ExecuteServiceLogic(
                async () => await _transactionService.GetTransactionByID(id).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetTransactionByOrderId/{id}")]
        [Authorize]
        public async Task<IActionResult> GetTransactionByOrderId(Guid id)
        {
            return await ExecuteServiceLogic(
                async () => await _transactionService.GetTransactionByOrderID(id).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetTransactionByShop")]
        [Authorize(Roles = UserRoleConstants.SHOP)]
        public async Task<IActionResult> GetTransactionByShop(Guid id, [FromQuery]SearchTransactionRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _transactionService.GetTransactionByShop(id, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetTransactionByUser")]
        [Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> GetTransactionByUser(Guid id, [FromQuery] SearchTransactionRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _transactionService.GetTransactionByUser(id, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetTotalTransaction")]
        [Authorize(Roles = UserRoleConstants.ADMIN + "," + UserRoleConstants.STAFF)]
        public async Task<IActionResult> GetTotalTransaction([FromQuery] TotalTRansactionRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _transactionService.GetTotalTransaction(request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpPost("ChangeTransactionStatus")]
        [Authorize(Roles = UserRoleConstants.ADMIN + "," + UserRoleConstants.STAFF)]
        public async Task<IActionResult> c([FromForm] ChangeTransactionStatus request)
        {
            return await ExecuteServiceLogic(
                async () => await _transactionService.ChangeTransactionStatus(request.Id, request.Status).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetTopShopHaveHighTransaction")]
        [Authorize(Roles = UserRoleConstants.ADMIN + "," + UserRoleConstants.STAFF)]
        public async Task<IActionResult> GetTopShopHaveHighTransaction([FromQuery] TopShopOrUserRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _transactionService.GetTopShopHaveHighTransaction(request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetTopUserHaveHighTransaction")]
        [Authorize(Roles = UserRoleConstants.ADMIN + "," + UserRoleConstants.STAFF)]
        public async Task<IActionResult> GetTopUserHaveHighTransaction([FromQuery]TopShopOrUserRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _transactionService.GetTopUserHaveHighTransaction(request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetTotalRevenue")]
        [Authorize(Roles = UserRoleConstants.ADMIN + "," + UserRoleConstants.STAFF)]
        public async Task<IActionResult> GetTotalRevenue([FromQuery] TotalTRansactionRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _transactionService.GetTotalRevenue(request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetTotalRevenueForShop/{shopId}")]
        [Authorize(Roles = UserRoleConstants.SHOP)]
        public async Task<IActionResult> GetTotalRevenueForShop(Guid shopId, [FromQuery] TotalTRansactionRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _transactionService.GetTotalRevenueForShop(shopId, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetTotalRevenueForUser")]
        [Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> GetTotalRevenueForUser([FromQuery] TotalTRansactionRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _transactionService.GetTotalRevenueForUser(_userClaims.UserId, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
    }
}
