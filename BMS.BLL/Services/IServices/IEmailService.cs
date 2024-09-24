using BMS.Core.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.IServices
{
    public interface IEmailService
    {
        Task SendEmailConfirmationAsync(User user, string token);
        Task SendEmailAsync(string ToEmail, string Subject, string Body, bool IsBodyHtml = false);

        Task SendEmailConfirmationMoblieAsync(User user, string token);
        Task SendEmailNotificationToShopAndUserAboutOrder(String url, string token, Order order, User user, Shop shop);
    }
}
