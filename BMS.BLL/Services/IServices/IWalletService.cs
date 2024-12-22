using BMS.BLL.Models;
using System.Transactions;

namespace BMS.BLL.Services.IServices
{
    public interface IWalletService
    {
        Task<ServiceActionResult> GetWalletByUserId(Guid userId);
        Task<ServiceActionResult> AddWallet(Guid userId);
        Task<ServiceActionResult> DeleteWallet(Guid userId);
        Task<ServiceActionResult> UpdateBalance(Guid userId, TransactionStatus status);
        Task<double> UpdateBalanceInSystem(Guid userId, TransactionStatus status);
    }
}
