﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Settings
{
    public class PayOSSettings
    {
        public string ClientId { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string ChecksumKey { get; set; } = string.Empty;
        public string CancelUrl { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
        public string WebhookUrl { get; set; } = string.Empty;
    }
}
