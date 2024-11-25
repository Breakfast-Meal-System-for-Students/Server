using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Models;
using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.BLL.Models.Responses.Admin;

namespace BMS.BLL.Services.IServices
{
    public interface ITransactionService
    {
        Task<ServiceActionResult> GetListTracsactions(SearchTransactionRequest request);
        Task<ServiceActionResult> GetTransactionByID(Guid id);
        Task<ServiceActionResult> GetTransactionByOrderID(Guid id);
        Task<ServiceActionResult> GetTransactionByUser(Guid id, SearchTransactionRequest request);
        Task<ServiceActionResult> GetTransactionByShop(Guid id, SearchTransactionRequest request);
        Task<ServiceActionResult> GetTopUserHaveHighTransaction(TopShopOrUserRequest request);
        Task<ServiceActionResult> GetTopShopHaveHighTransaction(TopShopOrUserRequest request);
        Task<ServiceActionResult> ChangeTransactionStatus(Guid id, TransactionStatus status);
        Task<ServiceActionResult> GetTotalTransaction(TotalTRansactionRequest request);

        Task<ServiceActionResult> AddTransaction(Guid orderId);
        Task<ServiceActionResult> UpdateTransaction(Guid transactionId, TransactionMethod transactionMethod = 0, TransactionStatus transactionStatus = 0);
        Task<ServiceActionResult> GetTotalRevenue(TotalTRansactionRequest request);
        Task<ServiceActionResult> GetTotalRevenueForShop(Guid shopId, TotalTRansactionRequest request);
        Task<ServiceActionResult> GetTotalRevenueForUser(Guid userId, TotalTRansactionRequest request);
    }
}
