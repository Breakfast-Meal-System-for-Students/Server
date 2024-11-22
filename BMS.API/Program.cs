using BMS.DAL.DataContext;
using Microsoft.EntityFrameworkCore;
using BMS.DAL;
using BMS.BLL;
using BMS.API.Extensions;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.SignalR;
using BMS.API.Hub;
using Microsoft.Extensions.FileProviders;
using System;

namespace BMS.API
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var app = BuildApplication(args);
            EnsureMigrate(app);
            app.Run();
        }

        public static WebApplication BuildApplication(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            ConfigureServices(builder);

            var app = builder.Build();

            // Configure middleware
            ConfigureMiddleware(app);

            return app;
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            });

            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.RegisterDALDependencies(builder.Configuration);
            builder.Services.RegisterBLLDependencies(builder.Configuration);
            builder.Services.AddVNPaySettings(builder.Configuration);
            builder.Services.AddPayOSSettings(builder.Configuration);
            builder.Services.AddCorsPolicy(builder.Configuration);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.DescribeAllParametersInCamelCase();
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddSignalR();
        }

        private static void ConfigureMiddleware(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BMS");
            });

            app.UseHttpsRedirection();
            app.UseCors("AllowReactApp");

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), ".well-known")),
                RequestPath = "/.well-known",
                ServeUnknownFileTypes = true,
                DefaultContentType = "application/json"
            });

            app.UseRouting();

            app.MapHub<CartHub>("/cartHub");
            app.MapHub<MyHub>("/myhub");
            app.MapHub<NotificationHub>("/notificationHub");
            app.MapHub<OrderHub>("/orderHub");

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
        }

        public static void EnsureMigrate(WebApplication webApp)
        {
            using var scope = webApp.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BMS_DbContext>();
            context.Database.Migrate();
        }
    }
}
