using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Exceptions
{
    public class MissingJwtSettingsException : ArgumentNullException
    {
        private readonly string? _customMessage;
        public override string Message => _customMessage ?? Message;

        public MissingJwtSettingsException(string customMessage)
        {
            _customMessage = customMessage;
        }

        public MissingJwtSettingsException()
        {
            _customMessage = "Missing jwt settings";
        }
    }
}
