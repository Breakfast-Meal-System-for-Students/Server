using Azure.Core;
using BMS.BLL.Services.IServices;
using BMS.BLL.Utilities;
using BMS.Core.Domains.Entities;
using BMS.Core.Exceptions;
using BMS.Core.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<User> _userManager;
        public TokenService(IConfiguration config, UserManager<User> userManager)
        {
            _key = GetSymmetricSecurityKey(config);
            _userManager = userManager;
        }
        public async Task<string> CreateToken(User user)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString())
        };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTimeHelper.GetCurrentTime().AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenToReturn = tokenHandler.WriteToken(token);

            return tokenToReturn;
        }

        public async Task<string> GenerateTokenForShareLink(Guid cartId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("CartID", cartId.ToString()),
                    new Claim("Type", "SharedCartAccess")
                }),
                Expires = DateTimeHelper.GetCurrentTime().AddHours(1),
                SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature)

        };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        private SymmetricSecurityKey GetSymmetricSecurityKey(IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>() ?? throw new MissingJwtSettingsException();
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey));

        }

        public async Task<string> CheckTokenForShareLink(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var cartIdFromToken = jwtToken.Claims.First(x => x.Type == "CartID").Value;
                return cartIdFromToken;

            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
