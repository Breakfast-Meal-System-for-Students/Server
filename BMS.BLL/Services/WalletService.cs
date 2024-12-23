using AutoMapper;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Basic;
using BMS.BLL.Models.Responses.Admin;
using BMS.BLL.Models.Responses.Cart;
using BMS.BLL.Models.Responses.Wallet;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using BMS.Core.Helpers;
using BMS.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services
{
    public class WalletService : BaseService, IWalletService
    {
        public readonly ITransactionService _transactionService;
        public WalletService(IUnitOfWork unitOfWork, IMapper mapper, ITransactionService transactionService) : base(unitOfWork, mapper)
        {
            _transactionService = transactionService;
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

        public async Task<ServiceActionResult> GetAllTransactionOfUserWallet(Guid userId, PagingRequest request)
        {
            var walletTransactions = (await _unitOfWork.WalletTransactionRepository.GetAllAsyncAsQueryable()).Include(a => a.Wallet).Where(x => x.Wallet.UserId == userId).OrderByDescending(y => y.CreateDate);
            var paginationResult = PaginationHelper
            .BuildPaginatedResult<WalletTransaction, WalletTransactionResponse>(_mapper, walletTransactions, request.PageSize, request.PageIndex);
            return new ServiceActionResult(true)
            {
                Data = paginationResult,
            };
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

        public async Task<ServiceActionResult> UpdateBalance(Guid userId, TransactionStatus status, decimal amount, Guid? orderId)
        {
            var wallet = (await _unitOfWork.WalletRepository.GetAllAsyncAsQueryable()).Where(x => x.UserId == userId && x.IsDeleted == false).FirstOrDefault();
            if (wallet == null)
            {
                return new ServiceActionResult(false)
                {
                    Detail = "The Wallet has been deleted or not exists"
                };
            }
            WalletTransaction walletTransaction = new WalletTransaction()
            {
                WalletID = wallet.Id,
                Status = status,
            };
            switch (status)
            {
                case TransactionStatus.PAID:
                    if(orderId == null)
                    {
                        return new ServiceActionResult(false)
                        {
                            Detail = "Since this is a payment transaction, please fill in the orderId"
                        };
                    }
                    if (wallet.Balance - amount <= 0)
                    {
                        return new ServiceActionResult(false)
                        {
                            Detail = "The balance of you can not enough to paid"
                        };
                    }
                    wallet.Balance -= amount;
                    walletTransaction.Price = (-amount);
                    await _unitOfWork.TransactionRepository.AddAsync(
                        new Transaction()
                            {
                                Method = TransactionMethod.BMSWallet.ToString(),
                                Status = TransactionStatus.PAID,
                                Price = (double)amount,
                                OrderId = (Guid)orderId,
                            }
                        );
                    await _unitOfWork.CommitAsync();
                    var y = await UpdateBalanceAdmin(TransactionStatus.PAID, amount);
                    if (y < 0)
                    {
                        return new ServiceActionResult(false)
                        {
                            Detail = "The System Wallet has been deleted or not exists. So The system can not recieved."
                        };
                    }
                    break;
                case TransactionStatus.REFUND:
                    if (orderId == null)
                    {
                        return new ServiceActionResult(false)
                        {
                            Detail = "Since this is a payment transaction, please fill in the orderId"
                        };
                    }
                    wallet.Balance += amount;
                    walletTransaction.Price = amount;
                    await _unitOfWork.TransactionRepository.AddAsync(
                        new Transaction()
                        {
                            Method = TransactionMethod.BMSWallet.ToString(),
                            Status = TransactionStatus.PAID,
                            Price = (double)(-amount),
                            OrderId = (Guid)orderId,
                        }
                        );
                    await _unitOfWork.CommitAsync();
                    break;
                case TransactionStatus.PAIDTOSHOP:
                    wallet.Balance += amount;
                    walletTransaction.Price = amount;
                    break;
                case TransactionStatus.DEPOSIT:
                    wallet.Balance += amount;
                    walletTransaction.Price = amount;
                    break;
                case TransactionStatus.WITHDRA:
                    if(wallet.Balance - amount <= 0)
                    {
                        return new ServiceActionResult(false)
                        {
                            Detail = "The balance of you can not enough to withdraw"
                        };
                    }
                    wallet.Balance -= amount;
                    walletTransaction.Price = (-amount);
                    break;
                case TransactionStatus.PAIDPACKAGE:
                    if (wallet.Balance - amount <= 0)
                    {
                        return new ServiceActionResult(false)
                        {
                            Detail = "The balance of you can not enough to PAIDPACKAGE"
                        };
                    }
                    wallet.Balance -= amount;
                    walletTransaction.Price = (-amount);
                    break;
            }
            await _unitOfWork.WalletRepository.UpdateAsync(wallet);
            await _unitOfWork.WalletTransactionRepository.AddAsync(walletTransaction);
            return new ServiceActionResult(true)
            {
                Data = wallet.Balance,
                Detail = "Add Wallet Transaction Succesfully"
            };
        }

        public async Task<decimal> UpdateBalanceInSystem(Guid userId, TransactionStatus status, decimal amount, Guid? orderId)
        {
            var wallet = (await _unitOfWork.WalletRepository.GetAllAsyncAsQueryable()).Where(x => x.UserId == userId && x.IsDeleted == false).FirstOrDefault();
            if (wallet == null)
            {
                return -1;
            }
            WalletTransaction walletTransaction = new WalletTransaction()
            {
                WalletID = wallet.Id,
                Status = status,
            };
            switch (status)
            {
                case TransactionStatus.PAID:
                    if (orderId == null)
                    {
                        return -1;
                    }
                    wallet.Balance -= amount;
                    walletTransaction.Price = (-amount);
                    await _unitOfWork.TransactionRepository.AddAsync(
                        new Transaction()
                        {
                            Method = TransactionMethod.BMSWallet.ToString(),
                            Status = TransactionStatus.PAID,
                            Price = (double)amount,
                            OrderId = (Guid)orderId,
                        }
                        );
                    break;
                case TransactionStatus.REFUND:
                    if (orderId == null)
                    {
                        return -1;
                    }
                    wallet.Balance += amount;
                    walletTransaction.Price = amount;
                    await _unitOfWork.TransactionRepository.AddAsync(
                        new Transaction()
                        {
                            Method = TransactionMethod.BMSWallet.ToString(),
                            Status = TransactionStatus.PAID,
                            Price = (double)(-amount),
                            OrderId = (Guid)orderId,
                        }
                        );
                    break;
                case TransactionStatus.PAIDTOSHOP:
                    wallet.Balance += amount;
                    walletTransaction.Price = amount;
                    break;
                case TransactionStatus.DEPOSIT:
                    wallet.Balance += amount;
                    walletTransaction.Price = amount;
                    break;
                case TransactionStatus.WITHDRA:
                    if (wallet.Balance - amount <= 0)
                    {
                        return -1;
                    }
                    wallet.Balance -= amount;
                    walletTransaction.Price = (-amount);
                    break;
            }
            await _unitOfWork.WalletRepository.UpdateAsync(wallet);
            await _unitOfWork.WalletTransactionRepository.AddAsync(walletTransaction);
            return wallet.Balance;
        }

        public async Task SaveChange()
        {
            await _unitOfWork.CommitAsync();
        }

        public async Task<decimal> UpdateBalanceAdmin(TransactionStatus status, decimal amount)
        {
            var wallet = (await _unitOfWork.WalletRepository.GetAllAsyncAsQueryable()).Include(a => a.User).Where(x => x.User.Email!.Equals("admin@gmail.com") && x.IsDeleted == false).FirstOrDefault();
            if (wallet == null)
            {
                return -1;
            }
            WalletTransaction walletTransaction = new WalletTransaction()
            {
                WalletID = wallet.Id,
                Status = status,
            };
            switch (status)
            {
                case TransactionStatus.PAID:
                    wallet.Balance += amount;
                    walletTransaction.Price = amount;
                    break;
                case TransactionStatus.REFUND:
                    wallet.Balance -= amount;
                    walletTransaction.Price = (-amount);
                    break;
                case TransactionStatus.PAIDTOSHOP:
                    wallet.Balance -= amount;
                    walletTransaction.Price = (-amount);
                    break;
                case TransactionStatus.DEPOSIT:
                    wallet.Balance += amount;
                    walletTransaction.Price = amount;
                    break;
                case TransactionStatus.WITHDRA:
                    if (wallet.Balance - amount <= 0)
                    {
                        return -1;
                    }
                    wallet.Balance -= amount;
                    walletTransaction.Price = (-amount);
                    break;
            }
            await _unitOfWork.WalletRepository.UpdateAsync(wallet);
            await _unitOfWork.WalletTransactionRepository.AddAsync(walletTransaction);
            return wallet.Balance;
        }

        public async Task<bool> CheckSystemPaidRevenueToShopInWeek(DateTime from, DateTime to, Guid userId)
        {
            var countOfPaidToShop = (await _unitOfWork.WalletTransactionRepository.GetAllAsyncAsQueryable()).Include(a => a.Wallet).Where(x => x.Wallet.UserId == userId && x.CreateDate > from && x.CreateDate < to && x.Status == TransactionStatus.PAIDTOSHOP).Count();
            if (countOfPaidToShop <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}