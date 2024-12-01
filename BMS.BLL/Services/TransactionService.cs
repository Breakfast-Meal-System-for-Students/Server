using AutoMapper;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Models.Responses.Admin;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
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
            IQueryable<Shop> shopQuery = (await _unitOfWork.ShopRepository.GetAllAsyncAsQueryable())
                                        .Include(a => a.Orders)
                                        .ThenInclude(b => b.Transactions);

            if (request.Year != 0)
            {
                if (request.Month != 0)
                {
                    shopQuery = shopQuery.Where(x => x.Orders
                        .All(order => order.Transactions
                            .All(y => y.CreateDate.Year == request.Year
                                      && y.CreateDate.Month == request.Month
                                      && y.Status == TransactionStatus.PAID)));
                }
                else
                {
                    shopQuery = shopQuery.Where(x => x.Orders
                        .All(order => order.Transactions
                            .All(y => y.CreateDate.Year == request.Year
                                      && y.Status == TransactionStatus.PAID)));
                }
            }

            // Nhóm theo ID và tính tổng giao dịch
            var shopTransactions = shopQuery
                .GroupBy(shop => shop.Id)
                .Select(group => new
                {
                    ShopId = group.Key,
                    ShopName = group.FirstOrDefault().Name,
                    ShopImage = group.FirstOrDefault().Image,
                    TotalTransactionAmount = group
                        .SelectMany(shop => shop.Orders)
                        .SelectMany(order => order.Transactions)
                        .Where(transaction => transaction.Status == TransactionStatus.PAID)  // Chỉ lấy những giao dịch đã thanh toán
                        .Sum(transaction => transaction.Price)
                })
                .OrderByDescending(x => x.TotalTransactionAmount); // Sắp xếp theo tổng giao dịch giảm dần

            // Phân trang kết quả
            var paginationResult = PaginationHelper.BuildPaginatedResult(shopTransactions, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };
        }

        public async Task<ServiceActionResult> GetTopUserHaveHighTransaction(TopShopOrUserRequest request)
        {
            IQueryable<User> users = (await _unitOfWork.UserRepository.GetAllAsyncAsQueryable())
                                    .Include(shop => shop.Orders)
                                    .ThenInclude(order => order.Transactions);
            if (request.Year != 0)
            {
                if (request.Month != 0)
                {
                    users = users.Where(x => x.Orders.All(x => x.Transactions.All(y => y.CreateDate.Year == request.Year && y.CreateDate.Month == request.Month && y.Status == TransactionStatus.PAID)));
                }
                else
                {
                    users = users.Where(x => x.Orders.All(x => x.Transactions.All(y => y.CreateDate.Year == request.Year && y.Status == TransactionStatus.PAID)));
                }
            }

            var usersTransaction = users
                        .GroupBy(user => user.Id)
                        .Select(group => new
                        {
                            Id = group.Key,
                            Name = group.FirstOrDefault().FirstName + group.FirstOrDefault().LastName,
                            Image = group.FirstOrDefault().Avatar,
                            TotalTransactionAmount = group
                                .SelectMany(shop => shop.Orders)
                                .SelectMany(order => order.Transactions)
                                .Sum(transaction => transaction.Price)
                        }).OrderByDescending(x => x.TotalTransactionAmount);
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
                    transaction.LastUpdateDate = DateTime.UtcNow;
                }
                if(transactionStatus != 0)
                {
                    transaction.Status = transactionStatus;
                    transaction.LastUpdateDate = DateTime.UtcNow;
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
