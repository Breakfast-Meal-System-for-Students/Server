using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Utilities
{
    public static class AccountCreationHelper
    {
        public static string GenerateRandomPassword()
        {
            const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
            const string numericChars = "0123456789";
            const string specialChars = "!@#$%^&*()_+";

            string allChars = upperChars + lowerChars + numericChars + specialChars;

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            // Ensure at least one character from each character set
            password.Append(upperChars[random.Next(upperChars.Length)]);
            password.Append(lowerChars[random.Next(lowerChars.Length)]);
            password.Append(numericChars[random.Next(numericChars.Length)]);

            // Fill the remaining characters randomly
            for (int i = 3; i < 8; i++)
            {
                password.Append(allChars[random.Next(allChars.Length)]);
            }

            // Shuffle the password characters
            string shuffledPassword = new string(password.ToString().OrderBy(c => random.Next()).ToArray());

            return shuffledPassword;
        }
    }
}
