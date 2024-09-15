using AutoMapper;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Models.Responses.Admin;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
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

        public async Task<ServiceActionResult> ChangeTransactionStatus(Guid id, TransactionStatus status)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceActionResult> GetListTracsactions(SearchTransactionRequest request)
        {
            IQueryable<Transaction> transactionQuery = (await _unitOfWork.TransactionRepository.GetAllAsyncAsQueryable()).Include(a => a.Order);

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
            var shops = (await _unitOfWork.ShopRepository.GetAllAsyncAsQueryable())
                        .Include(shop => shop.Orders)
                        .ThenInclude(order => order.Transactions);
                        //.GroupBy(shop => shop.Id)
                        //.Select(group => new
                        //{
                        //    ShopId = group.Key,
                        //    ShopName = group.FirstOrDefault().Name,
                        //    TotalTransactionAmount = group
                        //        .SelectMany(shop => shop.Orders)
                        //        .SelectMany(order => order.Transactions)
                        //        .Sum(transaction => transaction.Price)
                        //});
            if (request.Year != 0)
            {
                if (request.Month != 0)
                {
                    shops = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Shop, ICollection<Transaction>>)shops.Where(x => x.Orders.All(x => x.Transactions.All(y => y.CreateDate.Year == request.Year && y.CreateDate.Month == request.Month && y.Status == TransactionStatus.PAID)));
                } else
                {
                    shops = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Shop, ICollection<Transaction>>)shops.Where(x => x.Orders.All(x => x.Transactions.All(y => y.CreateDate.Year == request.Year && y.Status == TransactionStatus.PAID)));
                }
            }

            shops = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Shop, ICollection<Transaction>>)shops
                        .GroupBy(shop => shop.Id)
                        .Select(group => new
                        {
                            ShopId = group.Key,
                            ShopName = group.FirstOrDefault().Name,
                            TotalTransactionAmount = group
                                .SelectMany(shop => shop.Orders)
                                .SelectMany(order => order.Transactions)
                                .Sum(transaction => transaction.Price)
                        }).OrderByDescending(x => x.TotalTransactionAmount);
            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Shop, TopResponse>(_mapper, shops, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };

        }

        public async Task<ServiceActionResult> GetTopUserHaveHighTransaction(TopShopOrUserRequest request)
        {
            var users = (await _unitOfWork.UserRepository.GetAllAsyncAsQueryable())
                        .Include(shop => shop.Orders)
                        .ThenInclude(order => order.Transactions);
            if (request.Year != 0)
            {
                if (request.Month != 0)
                {
                    users = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<User, ICollection<Transaction>>)users.Where(x => x.Orders.All(x => x.Transactions.All(y => y.CreateDate.Year == request.Year && y.CreateDate.Month == request.Month && y.Status == TransactionStatus.PAID)));
                }
                else
                {
                    users = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<User, ICollection<Transaction>>)users.Where(x => x.Orders.All(x => x.Transactions.All(y => y.CreateDate.Year == request.Year && y.Status == TransactionStatus.PAID)));
                }
            }

            users = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<User, ICollection<Transaction>>)users
                        .GroupBy(shop => shop.Id)
                        .Select(group => new
                        {
                            Id = group.Key,
                            Name = group.FirstOrDefault().FirstName + group.FirstOrDefault().LastName,
                            TotalTransactionAmount = group
                                .SelectMany(shop => shop.Orders)
                                .SelectMany(order => order.Transactions)
                                .Sum(transaction => transaction.Price)
                        }).OrderByDescending(x => x.TotalTransactionAmount);
            var paginationResult = PaginationHelper
            .BuildPaginatedResult<User, TopResponse>(_mapper, users, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };
        }

        public async Task<ServiceActionResult> GetTotalTransaction(TotalTRansactionRequest request)
        {
            var transactions = (await _unitOfWork.OrderRepository.GetAllAsyncAsQueryable());
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
                transactions = transactions.Where(x => x.Status.Equals(request.Status.ToString()));
            }
            return new ServiceActionResult()
            {
                Data = transactions.Count()
            };
        }

        public async Task<ServiceActionResult> GetTransactionByID(Guid id)
        {
            var transaction = (await _unitOfWork.TransactionRepository.GetAllAsyncAsQueryable()).Include(a => a.Order).Where(x => x.Id == id);
            if (transaction != null)
            {
                var returnOrder = _mapper.Map<OrderResponse>(transaction);

                return new ServiceActionResult(true) { Data = returnOrder };
            }
            else
            {
                return new ServiceActionResult(false, "Transaction is not exits or deleted");
            }
        }

        public async Task<ServiceActionResult> GetTransactionByOrderID(Guid id)
        {
            var transaction = (await _unitOfWork.TransactionRepository.GetAllAsyncAsQueryable()).Include(a => a.Order).Where(x => x.OrderId == id);
            if (transaction != null)
            {
                var returnOrder = _mapper.Map<OrderResponse>(transaction);

                return new ServiceActionResult(true) { Data = returnOrder };
            }
            else
            {
                return new ServiceActionResult(false, "Transaction is not exits or deleted");
            }
        }

        public async Task<ServiceActionResult> GetTransactionByShop(Guid id, SearchTransactionRequest request)
        {
            IQueryable<Transaction> transactionQuery = (await _unitOfWork.TransactionRepository.GetAllAsyncAsQueryable()).Include(a => a.Order).Where(x => x.Order.ShopId == id);

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
            IQueryable<Transaction> transactionQuery = (await _unitOfWork.TransactionRepository.GetAllAsyncAsQueryable()).Include(a => a.Order).Where(x => x.Order.CustomerId == id);

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
    }
}
