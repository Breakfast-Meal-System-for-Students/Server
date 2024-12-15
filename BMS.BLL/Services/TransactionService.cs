using AutoMapper;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Models.Responses.Admin;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.BLL.Utilities;
using BMS.Core.Domains.Constants;
using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using BMS.Core.Helpers;
using BMS.DAL;
using BMS.DAL.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services
{
    public class TransactionService : BaseService, ITransactionService
    {
        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<ServiceActionResult> AddTransaction(Guid orderId)
        {
            var order = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable()).Where(x => x.Id == orderId).Include(y => y.OrderItems).SingleOrDefault();
            if (order == null) 
            {
                return new ServiceActionResult() { Detail = $"Order is not exits or deleted" }; 
            }
            Transaction transaction = new Transaction();
            transaction.OrderId = orderId;
            transaction.Status = TransactionStatus.NOTPAID;
            transaction.Method = TransactionMethod.Cash.ToString();
            transaction.Price = order.TotalPrice;
            await _unitOfWork.TransactionRepository.AddAsync(transaction);
            return new ServiceActionResult() { Detail = "Transaction is already create" };
        }

        public async Task<ServiceActionResult> ChangeTransactionStatus(Guid id, TransactionStatus status)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceActionResult> GetListTracsactions(SearchTransactionRequest request)
        {
            IQueryable<Transaction> transactionQuery = (await _unitOfWork.TransactionRepository.GetAllAsyncAsQueryable())
                                                        .Include(a => a.Order)
                                                            .ThenInclude(order => order.Customer)
                                                        .Include(a => a.Order)
                                                            .ThenInclude(order => order.Shop);

            //var canParsed = Enum.TryParse(request.Status, true, out OrderStatus status);
            if (request.Status != 0)
            {
                transactionQuery = transactionQuery.Where(m => m.Status.Equals(request.Status));
            }

            //if (!string.IsNullOrEmpty(request.Search))
            //{
            //    orderQuery = orderQuery.Where(m => m.Email.Contains(request.Search) || (m.LastName + m.FirstName).Contains(request.Search));
            //}

            transactionQuery = request.IsDesc ? transactionQuery.OrderByDescending(a => a.CreateDate) : transactionQuery.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Transaction, TransactionResponse>(_mapper, transactionQuery, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };
        }

        public async Task<ServiceActionResult> GetTopShopHaveHighTransaction(TopShopOrUserRequest request)
        {
            var shopQuery = (await _unitOfWork.ShopRepository.GetAllAsyncAsQueryable())
                            .Include(shop => shop.Orders)
                            .ThenInclude(order => order.Transactions);

            var filteredShops = shopQuery.Select(shop => new
            {
                ShopId = shop.Id,
                ShopName = shop.Name,
                ShopImage = shop.Image,
                TotalTransactionAmount = shop.Orders
                    .SelectMany(order => order.Transactions)
                    .Where(transaction =>
                        transaction.Status == TransactionStatus.PAID &&
                        (request.Year == 0 || transaction.CreateDate.Year == request.Year) &&
                        (request.Month == 0 || transaction.CreateDate.Month == request.Month))
                    .Sum(transaction => transaction.Price)
            });

            var shopTransactions = filteredShops
                .Where(s => s.TotalTransactionAmount > 0)
                .OrderByDescending(s => s.TotalTransactionAmount);

            var paginatedResult = PaginationHelper.BuildPaginatedResult(shopTransactions, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginatedResult };
        }



        public async Task<ServiceActionResult> GetTopUserHaveHighTransaction(TopShopOrUserRequest request)
        {
            IQueryable<User> users = (await _unitOfWork.UserRepository.GetAllAsyncAsQueryable())
                                    .Include(shop => shop.Orders)
                                    .ThenInclude(order => order.Transactions);

            var filteredUsers = users.Select(user => new
            {
                User = user,
                Transactions = user.Orders
                    .SelectMany(order => order.Transactions)
                    .Where(transaction =>
                        transaction.Status == TransactionStatus.PAID &&
                        (request.Year == 0 || transaction.CreateDate.Year == request.Year) &&
                        (request.Month == 0 || transaction.CreateDate.Month == request.Month))
            });

            var usersTransaction = filteredUsers
                .Select(u => new
                {
                    Id = u.User.Id,
                    Name = $"{u.User.FirstName} {u.User.LastName}",
                    Image = u.User.Avatar,
                    TotalTransactionAmount = u.Transactions.Sum(transaction => transaction.Price)
                })
                .OrderByDescending(x => x.TotalTransactionAmount);

            var paginationResult = PaginationHelper.BuildPaginatedResult(usersTransaction, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };
        }

        public async Task<ServiceActionResult> GetTotalRevenue(TotalTRansactionRequest request)
        {
            var transactions = (await _unitOfWork.TransactionRepository.GetAllAsyncAsQueryable());
            if (request.Year != 0)
            {
                if (request.Month != 0)
                {
                    if (request.Day != 0)
                    {
                        transactions = transactions.Where(x => x.CreateDate.Year == request.Year && x.CreateDate.Month == request.Month && x.CreateDate.Day == request.Day);
                    }
                    else
                    {
                        transactions = transactions.Where(x => x.CreateDate.Year == request.Year && x.CreateDate.Month == request.Month);
                    }
                }
                else
                {
                    transactions = transactions.Where(x => x.CreateDate.Year == request.Year);
                }
            }
            /*if (request.Status != 0)
            {
                transactions = transactions.Where(x => x.Status.Equals(request.Status));
            }*/
            return new ServiceActionResult()
            {
                Data = transactions.Where(x => x.Status.Equals(TransactionStatus.PAID)).Sum(x => x.Price)
            };
        }

        public async Task<ServiceActionResult> GetTotalRevenueForShop(Guid shopId, TotalTRansactionRequest request)
        {
            var transactions = (await _unitOfWork.TransactionRepository.GetAllAsyncAsQueryable()).Include(a => a.Order).Where(x => x.Order.ShopId == shopId);
            if (request.Year != 0)
            {
                if (request.Month != 0)
                {
                    if (request.Day != 0)
                    {
                        transactions = transactions.Where(x => x.CreateDate.Year == request.Year && x.CreateDate.Month == request.Month && x.CreateDate.Day == request.Day);
                    }
                    else
                    {
                        transactions = transactions.Where(x => x.CreateDate.Year == request.Year && x.CreateDate.Month == request.Month);
                    }
                }
                else
                {
                    transactions = transactions.Where(x => x.CreateDate.Year == request.Year);
                }
            }
            /*if (request.Status != 0)
            {
                transactions = transactions.Where(x => x.Status.Equals(request.Status));
            }*/
            return new ServiceActionResult()
            {
                Data = transactions.Where(x => x.Status.Equals(TransactionStatus.PAID)).Sum(x => x.Price)
            };
        }

        public async Task<ServiceActionResult> GetTotalRevenueForUser(Guid userId, TotalTRansactionRequest request)
        {
            var transactions = (await _unitOfWork.TransactionRepository.GetAllAsyncAsQueryable()).Include(a => a.Order).Where(x => x.Order.CustomerId == userId);
            if (request.Year != 0)
            {
                if (request.Month != 0)
                {
                    if (request.Day != 0)
                    {
                        transactions = transactions.Where(x => x.CreateDate.Year == request.Year && x.CreateDate.Month == request.Month && x.CreateDate.Day == request.Day);
                    }
                    else
                    {
                        transactions = transactions.Where(x => x.CreateDate.Year == request.Year && x.CreateDate.Month == request.Month);
                    }
                }
                else
                {
                    transactions = transactions.Where(x => x.CreateDate.Year == request.Year);
                }
            }
            /*if (request.Status != 0)
            {
                transactions = transactions.Where(x => x.Status.Equals(request.Status));
            }*/
            return new ServiceActionResult()
            {
                Data = transactions.Where(x => x.Status.Equals(TransactionStatus.PAID)).Sum(x => x.Price)
            };
        }

        public async Task<ServiceActionResult> GetTotalTransaction(TotalTRansactionRequest request)
        {
            var transactions = (await _unitOfWork.TransactionRepository.GetAllAsyncAsQueryable());
            if (request.Year != 0)
            {
                if (request.Month != 0)
                {
                    if (request.Day != 0)
                    {
                        transactions = transactions.Where(x => x.CreateDate.Year == request.Year && x.CreateDate.Month == request.Month && x.CreateDate.Day == request.Day);
                    }
                    else
                    {
                        transactions = transactions.Where(x => x.CreateDate.Year == request.Year && x.CreateDate.Month == request.Month);
                    }
                }
                else
                {
                    transactions = transactions.Where(x => x.CreateDate.Year == request.Year);
                }
            }
            if (request.Status != 0)
            {
                transactions = transactions.Where(x => x.Status.Equals(request.Status));
            }
            return new ServiceActionResult()
            {
                Data = transactions.Count()
            };
        }

        public async Task<ServiceActionResult> GetTransactionByID(Guid id)
        {
            var transaction = (await _unitOfWork.TransactionRepository.GetAllAsyncAsQueryable())
                                .Include(a => a.Order)
                                    .ThenInclude(order => order.Customer)
                                .Include(a => a.Order)
                                    .ThenInclude(order => order.Shop)
                                .Where(x => x.Id == id).FirstOrDefault();
            if (transaction != null)
            {
                var returnTransaction = _mapper.Map<TransactionResponse>(transaction);

                return new ServiceActionResult(true) { Data = returnTransaction };
            }
            else
            {
                return new ServiceActionResult(false, "Transaction is not exits or deleted");
            }
        }

        public async Task<ServiceActionResult> GetTransactionByOrderID(Guid id)
        {
            var transaction = (await _unitOfWork.TransactionRepository.GetAllAsyncAsQueryable())
                                .Include(a => a.Order)
                                    .ThenInclude(order => order.Customer)
                                .Include(a => a.Order)
                                    .ThenInclude(order => order.Shop)
                                .Where(x => x.OrderId == id).ToList();
            if (transaction != null)
            {
                var returnOrder = _mapper.Map<List<TransactionResponse>>(transaction);

                return new ServiceActionResult(true) { Data = returnOrder };
            }
            else
            {
                return new ServiceActionResult(false, "Transaction is not exits or deleted");
            }
        }

        public async Task<ServiceActionResult> GetTransactionByShop(Guid id, SearchTransactionRequest request)
        {
            IQueryable<Transaction> transactionQuery = (await _unitOfWork.TransactionRepository.GetAllAsyncAsQueryable())
                                                        .Include(a => a.Order)
                                                            .ThenInclude(order => order.Customer)
                                                        .Include(a => a.Order)
                                                            .ThenInclude(order => order.Shop)
                                                        .Where(x => x.Order.ShopId == id);

            //var canParsed = Enum.TryParse(request.Status, true, out OrderStatus status);
            if (request.Status != 0)
            {
                transactionQuery = transactionQuery.Where(m => m.Status.Equals(request.Status));
            }

            //if (!string.IsNullOrEmpty(request.Search))
            //{
            //    orderQuery = orderQuery.Where(m => m.Email.Contains(request.Search) || (m.LastName + m.FirstName).Contains(request.Search));
            //}

            transactionQuery = request.IsDesc ? transactionQuery.OrderByDescending(a => a.CreateDate) : transactionQuery.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Transaction, TransactionResponse>(_mapper, transactionQuery, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };
        }

        public async Task<ServiceActionResult> GetTransactionByUser(Guid id, SearchTransactionRequest request)
        {
            IQueryable<Transaction> transactionQuery = (await _unitOfWork.TransactionRepository.GetAllAsyncAsQueryable())
                                                        .Include(a => a.Order)
                                                            .ThenInclude(order => order.Customer)
                                                        .Include(a => a.Order)
                                                            .ThenInclude(order => order.Shop)
                                                        .Where(x => x.Order.CustomerId == id);

            //var canParsed = Enum.TryParse(request.Status, true, out OrderStatus status);
            if (request.Status != 0)
            {
                transactionQuery = transactionQuery.Where(m => m.Status.Equals(request.Status));
            }

            //if (!string.IsNullOrEmpty(request.Search))
            //{
            //    orderQuery = orderQuery.Where(m => m.Email.Contains(request.Search) || (m.LastName + m.FirstName).Contains(request.Search));
            //}

            transactionQuery = request.IsDesc ? transactionQuery.OrderByDescending(a => a.CreateDate) : transactionQuery.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Transaction, TransactionResponse>(_mapper, transactionQuery, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };
        }

        public async Task<ServiceActionResult> UpdateTransaction(Guid transactionId, TransactionMethod transactionMethod, TransactionStatus transactionStatus)
        {
            var transaction = (await _unitOfWork.TransactionRepository.GetAllAsyncAsQueryable()).Include(a => a.Order).Where(x => x.Id == transactionId).FirstOrDefault();
            if (transaction != null)
            {
                if(transactionMethod != 0)
                {
                    transaction.Method = transactionMethod.ToString();
                    transaction.LastUpdateDate = DateTimeHelper.GetCurrentTime();
                }
                if(transactionStatus != 0)
                {
                    transaction.Status = transactionStatus;
                    transaction.LastUpdateDate = DateTimeHelper.GetCurrentTime();
                }

                await _unitOfWork.TransactionRepository.UpdateAsync(transaction);
                return new ServiceActionResult(true) { Detail = $"Transaction {transactionId} is already update" };
            }
            else
            {
                return new ServiceActionResult(false, "Transaction is not exits or deleted");
            }
        }
    }
}
