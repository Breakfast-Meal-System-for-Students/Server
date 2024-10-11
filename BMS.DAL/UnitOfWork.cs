using BMS.DAL.Repositories.IRepositories;
using BMS.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace BMS.DAL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public IUserRepository UserRepository => new UserRepository(_dbContext);
        public ICategoryRepository CategoryRepositoy => new CategoryRepository(_dbContext);
        public IShopRepository ShopRepository => new ShopRepository(_dbContext);
        public IFeedbackRepository FeedbackRepository => new FeedbackRepository(_dbContext);
        public ITransactionRepository TransactionRepository => new TransactionRepository(_dbContext);
        public IOrderRepository OrderRepository => new OrderRepository(_dbContext);
        public IProductRepository ProductRepository => new ProductRepository(_dbContext);
        public ICartRepository CartRepository => new CartRepository(_dbContext);
        public ICartDetailRepository CartDetailRepository => new CartDetailRepository(_dbContext);
        public INotificationRepository NotificationRepository => new NotificationRepository(_dbContext);
        public IOrderItemRepository OrderItemRepository => new OrderItemRepository(_dbContext);
        public ICouponUsageRepository CouponUsageRepository => new CouponUsageRepository(_dbContext);
        public ICouponRepository CouponRepository => new CouponRepository(_dbContext);
        public IRegisterCategoryRepository RegisterCategoryRepository => new RegisterCategoryRepository(_dbContext);
        public IPackageRepository PackageRepository => new PackageRepository(_dbContext);
        public IPackageHistoryRepository PackageHistoryRepository => new PackageHistoryRepository(_dbContext);
        public ICartGroupUserRepository CartGroupUserRepository => new CartGroupUserRepository(_dbContext);
        public DbContext _dbContext { get; }
        
        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            try
            {
                GC.SuppressFinalize(this);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Rollback()
        {
            Console.WriteLine("Transaction rollback");
        }

        public async Task RollbackAsync()
        {
            Console.WriteLine("Transaction rollback");
            await Task.CompletedTask;
        }

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }
    }
}
