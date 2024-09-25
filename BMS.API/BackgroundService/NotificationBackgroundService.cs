using AutoMapper;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using BMS.DAL;
using BMS.DAL.DataContext;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

public class NotificationBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public NotificationBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Thực hiện việc kiểm tra và gửi thông báo
            await ProcessNotifications();

            // Đợi 30 phút rồi tiếp tục
            await Task.Delay(TimeSpan.FromMinutes(60*24), stoppingToken);
        }
    }

    private async Task ProcessNotifications()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            // Lấy tất cả các thông báo chưa gửi và có thời gian gửi đến
            var notifications = await notificationService.GetAllNotificationsToSendMail(NotificationStatus.Draft);

            foreach (var notification in notifications)
            {
                // Gửi email

                await emailService.SendEmailNotificationToShopAndUserAboutOrder("", "", notification.Order, notification.User, notification.Shop);

                // Đánh dấu thông báo đã được gửi
                notification.Status = NotificationStatus.Sended.ToString();
            }

            // Lưu thay đổi vào DB
            await notificationService.SaveChange();
        }
    }
}
