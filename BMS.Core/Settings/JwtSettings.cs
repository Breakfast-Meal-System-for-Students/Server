using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Settings
{
    public class JwtSettings
    {
        public string Issuer { get; init; }
        public string Audience { get; init; }
        public string SigningKey { get; init; }
        public bool ValidateAudience { get; init; }
        public bool ValidateIssuer { get; init; }
        public bool ValidateIssuerSigningKey { get; init; }
        public bool ValidateLifetime { get; init; }
        public int AccessTokenLifetimeInMinutes { get; init; }
        public int RefreshTokenLifetimeInMinutes { get; init; }
    }
}
