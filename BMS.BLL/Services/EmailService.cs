﻿using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BMS.BLL.Helpers;

namespace BMS.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {

            _configuration = configuration;
            _httpContextAccessor = new HttpContextAccessor();
            _urlHelperFactory = new UrlHelperFactory();

        }

        public async Task SendEmailAsync(string ToEmail, string Subject, string Body, bool IsBodyHtml = false)
        {
            var MailServer = _configuration["EmailSettings:MailServer"];
            var FromEmail = _configuration["EmailSettings:FromEmail"];
            var Password = _configuration["EmailSettings:Password"];
            var Port = int.Parse(_configuration["EmailSettings:MailPort"]);

            var client = new SmtpClient(MailServer, Port)
            {
                Credentials = new NetworkCredential(FromEmail, Password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage(FromEmail, ToEmail, Subject, EmailHelper.CreateEmailBody(Body, Subject))
            {
                IsBodyHtml = IsBodyHtml
            };
            await client.SendMailAsync(mailMessage);
        }

        public async Task SendEmailConfirmationAsync(User user, string token = "")
        {
            //var urlHelper = _urlHelperFactory.GetUrlHelper(new ActionContext(_httpContextAccessor.HttpContext, new RouteData(), new ActionDescriptor()));
            //var confirmationLink = urlHelper.Action("ConfirmEmail", "Auth", new { userId = user.Id, token }, _httpContextAccessor.HttpContext.Request.Scheme);
            //will improve this later
            var encodedToken = WebUtility.UrlEncode(token);
            await SendEmailAsync(user.Email, "Confirm Your Account Registration", EmailHelper.GetConfirmEmailBody($"http://localhost:3000/home?userId={user.Id}&token={encodedToken}", user.UserName, "breakfastmealsystem@gmail.com"), true);
        }
        public async Task SendEmailConfirmationMoblieAsync(User user, string token = "")
        {
     
            var encodedToken = WebUtility.UrlEncode(token);
            await SendEmailAsync(user.Email, "Confirm Your Account Registration", EmailHelper.GetConfirmEmailBody($"https://localhost:7039/api/Auth/confirm-email?userId={user.Id}&token={encodedToken}", user.UserName, "breakfastmealsystem@gmail.com"), true);
        }

    }
}