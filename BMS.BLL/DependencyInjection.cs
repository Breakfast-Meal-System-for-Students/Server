using BMS.BLL.Services.IServices;
using BMS.BLL.Services;
using BMS.BLL.Utilities.AutoMapperProfiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using FluentValidation;
using BMS.BLL.Validators;
using BMS.DAL.Repositories.IRepositories;
using BMS.BLL.AI;

namespace BMS.BLL
{
    public static class DependencyInjection
    {
        public static void RegisterBLLDependencies(this IServiceCollection services, IConfiguration Configuration)
        {
            
            services.AddAutoMapper(typeof(AutoMapperProfiles));

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICookieService, CookieService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserClaimsService, UserClaimsService>();


            // Register HttpClient
            services.AddHttpClient();


            #region Service
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICookieService, CookieService>();
            services.AddScoped<IShopApplicationService, ShopApplicationService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IFileStorageService, FileStorageService>();

            services.AddScoped<IShopApplicationService, ShopApplicationService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IRegisterCategoryService, RegisterCategoryService>();
            services.AddScoped<ICouponService, CouponService>();
            services.AddScoped<IPackageService, PackageService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IVnPayService, VnPayService>();
            services.AddScoped<IPayOSService, PayOSService>();
            services.AddScoped<IQRCodeService, QRCodeService>();
            services.AddScoped<ICartGroupUserService, CartGroupUserService>();
            services.AddScoped<IShopService, ShopService>();
            services.AddScoped<IGoogleMapService, GoogleMapService>();
            services.AddScoped<IShopWeeklyReportService, ShopWeeklyReportService>();
            services.AddScoped<IOpeningHoursService, OpeningHoursService>();
            services.AddScoped<IFavouriteService, FavouriteService>();
            services.AddScoped<IRecommendationService, RecommendationService>();
            #endregion

            #region
            services.AddSingleton<RecommendationEngine>();
            #endregion

            #region Validation
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<UserToLoginDTOValidator>();
       


            #endregion

        }
    }
}
