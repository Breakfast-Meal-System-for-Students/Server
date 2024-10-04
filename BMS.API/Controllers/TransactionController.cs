using BMS.API.Controllers.Base;
using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Services;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
    public class TransactionController : BaseApiController
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
            _baseService = (BaseService)transactionService;
        }

        [HttpGet("GetListTransactions")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> GetListTransactions([FromQuery]SearchTransactionRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _transactionService.GetListTracsactions(request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetTransactionById{id}")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> GetTransactionById(Guid id)
        {
            return await ExecuteServiceLogic(
                async () => await _transactionService.GetTransactionByID(id).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetTransactionByOrderId{id}")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> GetTransactionByOrderId(Guid id)
        {
            return await ExecuteServiceLogic(
                async () => await _transactionService.GetTransactionByOrderID(id).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetTransactionByShop")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> GetTransactionByShop(Guid id, [FromQuery]SearchTransactionRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _transactionService.GetTransactionByShop(id, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetTransactionByUser")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> GetTransactionByUser(Guid id, [FromQuery] SearchTransactionRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _transactionService.GetTransactionByUser(id, request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetTotalTransaction")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> GetTotalTransaction([FromQuery] TotalTRansactionRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _transactionService.GetTotalTransaction(request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpPost("ChangeTransactionStatus")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> ChangeTransactionStatus(Guid id, TransactionStatus status)
        {
            return await ExecuteServiceLogic(
                async () => await _transactionService.ChangeTransactionStatus(id, status).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetTopShopHaveHighTransaction")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> GetTopShopHaveHighTransaction([FromQuery] TopShopOrUserRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _transactionService.GetTopShopHaveHighTransaction(request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [HttpGet("GetTopUserHaveHighTransaction")]
        //[Authorize(Roles = UserRoleConstants.ADMIN)]
        public async Task<IActionResult> GetTopUserHaveHighTransaction([FromQuery]TopShopOrUserRequest request)
        {
            return await ExecuteServiceLogic(
                async () => await _transactionService.GetTopUserHaveHighTransaction(request).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
    }
}
