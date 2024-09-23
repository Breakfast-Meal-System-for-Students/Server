using BMS.DAL.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.DAL
{
    public static class DependencyInjection
    {
        public static void RegisterDALDependencies(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped<Microsoft.EntityFrameworkCore.DbContext, BMS_DbContext>();
            services.AddDbContext<BMS_DbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")
                    , sqlOptions =>
                    {
                        sqlOptions.MaxBatchSize(100); // Default value is 100, use a higher value to optimize
                        sqlOptions.CommandTimeout(60); // Still keep timeout to avoid timeouts on larger queries
                    });
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
