using BMS.Core.Exceptions.IExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Exceptions
{
    public class AddRoleException : ArgumentException, IBusinessException
    {
        private readonly string? _customMessage;
        public override string Message => _customMessage ?? Message;

        public AddRoleException(string customMessage)
        {
            _customMessage = customMessage;
        }

        public AddRoleException()
        {
            _customMessage = "Can add account with shop role";
        }
    }
}
