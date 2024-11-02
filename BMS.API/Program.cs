using BMS.DAL.DataContext;
using Microsoft.EntityFrameworkCore;
using BMS.DAL;
using BMS.BLL;
using BMS.API.Extensions;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.SignalR;
using BMS.API.Hub;
using Microsoft.Extensions.FileProviders;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true; // Optional: For pretty-printed JSON
}); ;
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.RegisterDALDependencies(builder.Configuration);
builder.Services.RegisterBLLDependencies(builder.Configuration);
builder.Services.AddVNPaySettings(builder.Configuration);
builder.Services.AddPayOSSettings(builder.Configuration);
builder.Services.AddCorsPolicy(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddSignalR();
var app = builder.Build();
EnsureMigrate(app);
// Configure the HTTP request pipeline.
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
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), ".well-known")),
    RequestPath = "/.well-known",
    ServeUnknownFileTypes = true,
    DefaultContentType = "application/json"
});
app.UseRouting();
// Serve static files from .well-known with JSON content type

app.MapHub<CartHub>("/cartHub");
app.MapHub<MyHub>("/myhub");
app.MapHub<NotificationHub>("/notificationHub");
app.MapHub<OrderHub>("/orderHub");
// Authentication and Authorization middleware should be placed here
app.UseAuthentication();  // Enable authentication middleware
app.UseAuthorization();   // Enable authorization middleware

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});

app.MapControllers();
app.UseStaticFiles();
app.Run();
void EnsureMigrate(WebApplication webApp)
{
    using var scope = webApp.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<BMS_DbContext>();
    context.Database.Migrate();
}