﻿using BMS.BLL.Services.IServices;
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

namespace BMS.BLL
{
    public static class DependencyInjection
    {
        public static void RegisterBLLDependencies(this IServiceCollection services, IConfiguration Configuration)
        {
            
            services.AddAutoMapper(typeof(AutoMapperProfiles));




            #region Service
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICookieService, CookieService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IUserClaimsService, UserClaimsService>();
            services.AddScoped<IShopApplicationService, ShopApplicationService>();
            services.AddScoped<IStaffService, StaffService>();
            #endregion

            #region Validation
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<UserToLoginDTOValidator>();
       


            #endregion

        }
    }
}
