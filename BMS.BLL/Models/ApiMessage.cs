using BMS.BLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models
{
    public class ApiMessage
    {
        public string Content { get; set; }
        public ApiMessageType Type { get; set; }

        public ApiMessage()
        {
            Content = string.Empty;
        }

        public ApiMessage(string content, ApiMessageType messageType = ApiMessageType.Info)
        {
            Content = content;
            Type = messageType;
        }
    }
}
