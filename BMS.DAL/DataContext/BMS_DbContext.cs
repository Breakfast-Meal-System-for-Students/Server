using BMS.Core.Domains.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.DAL.DataContext
{

    public class BMS_DbContext : IdentityDbContext<User, Role,
   Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>,
   IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public BMS_DbContext(DbContextOptions<BMS_DbContext> options) : base(options)
        {
        }

        // DbSet properties for all tables
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<RegisterCategory> RegisterCategorys { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageHistory> PackageHistories { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CouponUsage> CouponUsages { get; set; }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }

        public DbSet<OpeningHours> OpeningHours { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // 1-to-1 relationship between Shop and user
            modelBuilder.Entity<User>()
      .HasOne(u => u.Shop) 
      .WithOne(s => s.User) 
      .HasForeignKey<Shop>(s => s.UserId) 
      .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });
                
                entity.HasOne(ur => ur.User)
                      .WithMany(u => u.UserRoles)
                      .HasForeignKey(ur => ur.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ur => ur.Role)
                      .WithMany(r => r.UserRoles)
                      .HasForeignKey(ur => ur.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            //
   
            // One-to-Many relationship between User and Order
            modelBuilder.Entity<RegisterCategory>()
                .HasOne(rc => rc.Category)
                      .WithMany(c => c.RegisterCategorys) // Navigational property in Category
                      .HasForeignKey(rc => rc.CategoryId)
                      .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many relationship between Shop and Order
            modelBuilder.Entity<RegisterCategory>()
                .HasOne(rc => rc.Product)
                      .WithMany(c => c.RegisterCategorys) // Navigational property in Category
                      .HasForeignKey(rc => rc.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            // One-to-Many relationship between User and Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many relationship between Shop and Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Shop)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.ShopId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many relationship between User and Notification
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many relationship between Order and Notification
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Order)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many relationship between Shop and Notification
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Shop)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.ShopId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many relationship between User and Feedback
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.User)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many relationship between Shop and Feedback
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Shop)
                .WithMany(s => s.Feedbacks)
                .HasForeignKey(f => f.ShopId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many relationship between Product and Image
            modelBuilder.Entity<Image>()
                .HasOne(i => i.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many relationship between Shop and Product
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Shop)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.ShopId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many relationship between Package and PackageHistory
            modelBuilder.Entity<PackageHistory>()
                .HasOne(ph => ph.Package)
                .WithMany(p => p.PackageHistories)
                .HasForeignKey(ph => ph.PackageId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many relationship between Shop and PackageHistory
            modelBuilder.Entity<PackageHistory>()
                .HasOne(ph => ph.Shop)
                .WithMany(s => s.PackageHistories)
                .HasForeignKey(ph => ph.ShopId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many relationship between Order and OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many relationship between Product and OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many relationship between Coupon and CouponUsage
            modelBuilder.Entity<CouponUsage>()
                .HasOne(cu => cu.Coupon)
                .WithMany(c => c.CouponUsages)
                .HasForeignKey(cu => cu.CouponId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many relationship between User and CouponUsage
            modelBuilder.Entity<CouponUsage>()
                .HasOne(cu => cu.User)
                .WithMany(u => u.CouponUsages)
                .HasForeignKey(cu => cu.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many relationship between Order and CouponUsage
            modelBuilder.Entity<CouponUsage>()
                .HasOne(cu => cu.Order)
                .WithMany(o => o.CouponUsages)
                .HasForeignKey(cu => cu.OrderId)
                .OnDelete(DeleteBehavior.Restrict);


            // Cấu hình quan hệ 1-n giữa Customer và Cart
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Customer)
                .WithMany(cu => cu.Carts)
                .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
 
            // One-to-Many relationship between Shop and Order
            modelBuilder.Entity<Cart>()
                .HasOne(o => o.Shop)
                .WithMany(s => s.Carts)
                .HasForeignKey(o => o.ShopId)
                .OnDelete(DeleteBehavior.Restrict);
            // Cấu hình quan hệ 1-n giữa Cart và CartDetail
            modelBuilder.Entity<CartDetail>()
                .HasOne(cd => cd.Cart)
                .WithMany(c => c.CartDetails)
                .HasForeignKey(cd => cd.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình quan hệ 1-n giữa Product và CartDetail
            modelBuilder.Entity<CartDetail>()
                .HasOne(cd => cd.Product)
                .WithMany(p => p.CartDetails)
                .HasForeignKey(cd => cd.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            // Thiết lập mối quan hệ giữa Transaction và Order
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Order)
                .WithMany(o => o.Transactions)  // Một đơn hàng có thể có nhiều giao dịch
                .HasForeignKey(t => t.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            // Relationship between OpenHouse and Shop (One-to-Many)
            modelBuilder.Entity<OpeningHours>()
                .HasOne(oh => oh.Shop)
                .WithMany(s => s.OpeningHours)
                .HasForeignKey(oh => oh.ShopId)
                .OnDelete(DeleteBehavior.Cascade);
            // seed dât
            modelBuilder.Entity<Role>().HasData(
          new Role { Id = Guid.NewGuid(), Name = "Admin", NormalizedName="Admin"},
        new Role { Id = Guid.NewGuid(), Name = "Staff", NormalizedName = "Staff" },
         new Role { Id = Guid.NewGuid(), Name = "User", NormalizedName = "User" },
          new Role { Id = Guid.NewGuid(), Name = "Shop", NormalizedName = "Shop" });

            modelBuilder.Entity<Category>().HasData(
          new Category { Id = Guid.NewGuid(), Name = "Rice",Description= "Rice" },
        new Category { Id = Guid.NewGuid(), Name = "SuShi", Description = "SuShi" });

        }
    }
}
