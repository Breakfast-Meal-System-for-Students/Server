using AutoMapper;
using BMS.BLL.Models;
using BMS.BLL.Models.Responses.Admin;
using BMS.BLL.Models.Responses.Wallet;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Entities;
using BMS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BMS.BLL.Services
{
    public class WalletService : BaseService, IWalletService
    {
        public WalletService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<ServiceActionResult> AddWallet(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceActionResult> DeleteWallet(Guid userId)
        {
            var wallet = (await _unitOfWork.WalletRepository.GetAllAsyncAsQueryable()).Where(x => x.UserId == userId && x.IsDeleted == false).FirstOrDefault();
            if (wallet == null)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "The Wallet has been deleted or not exists"
                };
            }
            await _unitOfWork.WalletRepository.SoftDeleteByIdAsync(wallet.Id);
            return new ServiceActionResult();
        }

        public async Task<ServiceActionResult> GetWalletByUserId(Guid userId)
        {
            var wallet = (await _unitOfWork.WalletRepository.GetAllAsyncAsQueryable()).Where(x => x.UserId == userId && x.IsDeleted == false).FirstOrDefault();
            if (wallet == null)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "The Wallet has been deleted or not exists"
                };
            }
            var returnWallet = _mapper.Map<WalletResponse>(wallet);
            return new ServiceActionResult(true)
            {
                Data = returnWallet,
            };
        }

        public async Task<ServiceActionResult> UpdateBalance(Guid userId, TransactionStatus status)
        {
            var wallet = (await _unitOfWork.WalletRepository.GetAllAsyncAsQueryable()).Where(x => x.UserId == userId && x.IsDeleted == false).FirstOrDefault();
            if (wallet == null)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "The Wallet has been deleted or not exists"
                };
            }
        }

        public async Task<double> UpdateBalanceInSystem(Guid userId, TransactionStatus status)
        {
            throw new NotImplementedException();
        }
    }
}
