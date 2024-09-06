using BMS.BLL.Exceptions.IExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Exceptions
{
    public class UserNotFoundException : ArgumentNullException, INotFoundException
    {
        private readonly string? _customMessage;
        public override string Message => _customMessage ?? Message;

        public UserNotFoundException(string customMessage)
        {
            _customMessage = customMessage;
        }

        public UserNotFoundException()
        {
            _customMessage = "User not found";
        }
    }
}
