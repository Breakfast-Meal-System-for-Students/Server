using BMS.Core.Exceptions.IExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Exceptions
{
    public class UnactivatedEmailException : ArgumentException, IForbiddenException
    {
        private readonly string? _customMessage;
        public override string Message => _customMessage ?? Message;

        public UnactivatedEmailException(string customMessage)
        {
            _customMessage = customMessage;
        }

        public UnactivatedEmailException()
        {
            _customMessage = "Email is not activated";
        }
    }
}
