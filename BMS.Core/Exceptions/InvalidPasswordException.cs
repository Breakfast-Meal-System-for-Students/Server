using BMS.Core.Exceptions.IExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Exceptions
{
    public class InvalidPasswordException : InvalidCredentialException, IBusinessException
    {
        private readonly string? _customMessage;
        public override string Message => _customMessage ?? Message;

        public InvalidPasswordException(string customMessage)
        {
            _customMessage = customMessage;
        }

        public InvalidPasswordException()
        {
            _customMessage = "Invalid Password";
        }
    }
}
