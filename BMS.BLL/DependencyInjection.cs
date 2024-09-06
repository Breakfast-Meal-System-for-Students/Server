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

namespace BMS.BLL
{
    public static class DependencyInjection
    {
        public static void RegisterBLLDependencies(this IServiceCollection services, IConfiguration Configuration)
        {
            ;
            services.AddAutoMapper(typeof(AutoMapperProfiles));

           
            
         
            services.AddScoped<IFeedbackService, FeedbackService>();
         

           // services.AddFluentValidationAutoValidation();
            //services.AddValidatorsFromAssemblyContaining<UserToLoginDTOValidator>();


        }
    }
}
