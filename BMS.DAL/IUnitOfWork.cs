using BMS.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.DAL
{
    public interface IUnitOfWork
    {
        public IFeedbackRepository FeedbackRepository { get; }
        public IUserRepository UserRepository { get; }
        public IShopRepository ShopRepository { get; }
        public ICategoryRepository CategoryRepositoy { get; }
        public ITransactionRepository TransactionRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public ICartRepository CartRepository { get; }
        public ICartDetailRepository CartDetailRepository { get; }
        public INotificationRepository NotificationRepository { get; }
        public IOrderItemRepository OrderItemRepository { get; }
        public ICouponUsageRepository CouponUsageRepository { get; }
        public ICouponRepository CouponRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IRegisterCategoryRepository RegisterCategoryRepository { get; }
        public IPackageRepository PackageRepository { get; }
        public IPackage_ShopRepository Package_ShopRepository { get; }
        public ICartGroupUserRepository CartGroupUserRepository { get; }
        public IShopWeeklyReportRepository ShopWeeklyReportRepository { get; }
        public IOpeningHoursRepository OpeningHoursRepository { get; }
        public IImageRepository ImageRepository { get; }
        public IFavouriteRepository FavoriteRepository { get; }
        void Commit();
        Task CommitAsync();
        void Rollback();
        Task RollbackAsync();

        Task<IDbContextTransaction> BeginTransaction();
    }
}
