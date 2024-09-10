using BMS.Core.Exceptions.IExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Exceptions
{
    public class BusinessRuleException : InvalidCredentialException, IBusinessException
    {
        private readonly string? _customMessage;
        public override string Message => _customMessage ?? Message;

        public BusinessRuleException(string customMessage)
        {
            _customMessage = customMessage;
        }

        public BusinessRuleException()
        {
            _customMessage = "Business rule is violated";
        }
    }
}
