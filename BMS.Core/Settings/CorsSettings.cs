using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Settings
{
    public class CorsSettings
    {
        public string PolicyName { get; set; } = "AllowCors";
        public string[] WithOrigins { get; set; } = Array.Empty<string>();
        public string[] WithHeaders { get; set; } = Array.Empty<string>();
        public string[] WithMethods { get; set; } = Array.Empty<string>();
        public bool AllowCredentials { get; set; }
    }
}
