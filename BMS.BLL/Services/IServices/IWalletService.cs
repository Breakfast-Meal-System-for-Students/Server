using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Basic;
using BMS.Core.Domains.Enums;

namespace BMS.BLL.Services.IServices
{
    public interface IWalletService
    {
        Task<ServiceActionResult> GetWalletByUserId(Guid userId);
        Task<ServiceActionResult> AddWallet(Guid userId);
        Task<ServiceActionResult> DeleteWallet(Guid userId);
        Task<ServiceActionResult> UpdateBalance(Guid userId, TransactionStatus status, decimal amount, Guid? orderId);
        Task<decimal> UpdateBalanceAdmin(TransactionStatus status, decimal amount);
        Task<ServiceActionResult> GetAllTransactionOfUserWallet(Guid userId, PagingRequest request);
        Task<decimal> UpdateBalanceInSystem(Guid userId, TransactionStatus status, decimal amount, Guid? orderId);
        Task SaveChange();
    }
}
