using BMS.BLL.Utilities;
using BMS.Core.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Helpers
{
    public class EmailHelper
    {
        public static string CreateEmailBody(string content, string title = "Default Title")
        {
            var emailBody = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <title>{title}</title>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        margin: 0;
                        padding: 0;
                        background-color: #f4f4f4;
                    }}
                    .container {{
                        width: 80%;
                        margin: auto;
                        background-color: #fff;
                        padding: 20px;
                        border-radius: 10px;
                        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    }}
                    .header {{
                        background-color: #FFD700;
                        color: white;
                        padding: 10px 0;
                        text-align: center;
                        border-radius: 10px 10px 0 0;
                    }}
                    .footer {{
                        background-color: #FFD700;
                        color: white;
                        padding: 10px 0;
                        text-align: center;
                        border-radius: 0 0 10px 10px;
                    }}
                    .content {{
                        padding: 20px;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>{title}</h1>
                    </div>
                    <div class='content'>
                        {content}
                    </div>
                    <div class='footer'>
                        <p>&copy; {DateTimeHelper.GetCurrentTime().Year} BMS. All rights reserved.</p>
                    </div>
                </div>
            </body>
            </html>";

            return emailBody;
        }

        public static string GetRejectedEmailBody(string userName, string companyName, string message)
        {
            return $@"Dear {userName},<br>

                        Thank you for your interest in the Shoper position at {companyName}. We appreciate the time and effort you invested in applying for this role.<br>

                        After careful consideration, we regret to inform you that we have decided not to move forward with your application. <br>

                        The reason is : {message} <br>                        

                        Thank you once again for considering {companyName}. We value your interest and hope you find success in your career endeavors.<br>

                        Best regards,<br>

                        {companyName}";
        }
        public static string GetRejectedEmailStudentBody(string userName, string companyName, string message)
        {
            return $@"Dear {userName},<br>

                        Thank you for your interest in the User position at {companyName}. We appreciate the time and effort you invested in applying for this role.<br>

                        After careful consideration, we regret to inform you that we have decided not to move forward with your application. <br>

                        The reason is : {message} <br>                        

                        Thank you once again for considering {companyName}<br>

                        Best regards,<br>

                        {companyName}";
        }
        public static string GetAcceptedEmailBody(string platformEmail, string account, string password)
        {
            return $@"Congratulations! 🎉 Your application has been accepted! We are thrilled to welcome you to become a shoper in our platform. 🤝<br>

                    We provide you account to become a shoper in our platform: <br>
                    Account: {account}<br>
                    Password: {password}<br>

                    After log in, please change your password. Make sure to choose a strong and secure password!<br>

                    If you have any questions or need additional information, feel free to reach out to support team at {platformEmail}.<br>

                    Once again, welcome aboard, and we look forward to working with you! 🌟<br>

                    Best regards,";
        }

        public static string GetEmailToSendPWToStaff(string platformEmail, string account, string password)
        {
            return $@"Welcome to the BMS Team! 🎉<br>

                    We are pleased to inform you that your staff account has been created successfully. Below are your login credentials for the BMS platform:<br>
                    <strong>Account:</strong> {account}<br>
                    <strong>Password:</strong> {password}<br>

                    Please make sure to log in and change your password as soon as possible to ensure the security of your account. Choose a strong and unique password to keep your information safe.<br>

                    If you encounter any issues or need assistance, feel free to contact our support team at {platformEmail}.<br>

                    Thank you for being part of the BMS team, and we look forward to working with you! 🌟<br>

                    Best regards,<br>
                    BMS Admin Team";

        }

        public static string GetConfirmEmailBody(string link, string userName, string platformEmail)
        {
            return $@"Dear {userName},

                        Thank you for registering with our platform! We’re excited to have you join our community. To complete your account setup, please follow the steps below:<br>

                        Verify Your Email Address: Click on the following link to verify your email address and activate your account: <a href='{link}'>here</a><br>
            
                        Explore Our Features: Once your account is activated, you can start exploring our platform. Discover exciting content, connect with other users, and enjoy our services.<br>  

                        If you did not register for an account, please ignore this email. Your privacy and security are important to us, and we will never share your information with third parties.<br>

                        If you encounter any issues or need assistance, feel free to reach out to our support team at {platformEmail}.";
        }

        public static string GetForgotPasswordEmailBody(string link, string userName)
        {
            return $@"Dear {userName},

                            We received a request to reset your password for your account. If you did not initiate this request, please ignore this email.

                            To reset your password, click on the link below:

                            <a href='{link}'>Reset Password</a><br>

                            If you encounter any issues, feel free to contact our support team.

                            Best regards,";
        }

        public static string GetNotificationMail(string link, string userName, string platformEmail, Order order)
        {
            return $@"Dear {userName},

                        Thank you for booking order {order.Id} with our platform! We’re excited to have you join our community. To complete your account setup, please follow the steps below:<br>

                        Verify Your Email Address: Click on the following link to verify your email address and activate your account: <a href='{link}'>here</a><br>
            
                        Explore Our Features: Once your account is activated, you can start exploring our platform. Discover exciting content, connect with other users, and enjoy our services.<br>  

                        If you did not register for an account, please ignore this email. Your privacy and security are important to us, and we will never share your information with third parties.<br>

                        If you encounter any issues or need assistance, feel free to reach out to our support team at {platformEmail}.";
        }
    }
}
