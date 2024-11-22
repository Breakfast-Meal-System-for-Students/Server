using iTextSharp.text.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.TEST.Utils
{
    public class Account
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public static Account GetAccount(int x)
        {
            switch (x)
            {
                case 0:
                    return new Account()
                    {
                        UserName = "user@gmail.com",
                        Password = "User123@"
                    };
                case 1:
                    return new Account()
                    {
                        UserName = "shop@gmail.com",
                        Password = "Shop123@"
                    };
                case 2:
                    return new Account()
                    {
                        UserName = "staff@gmail.com",
                        Password = "Staff123@"
                    };
                case 3:
                    return new Account()
                    {
                        UserName = "staff@gmail.com",
                        Password = "Staff123@"
                    };
                default:
                    throw new Exception();
            }
        }
    }
}
