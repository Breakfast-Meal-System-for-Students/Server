using BMS.BLL.Services.IServices;
using BMS.BLL.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services
{
    public class CookieService : ICookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public CookieService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public void SetJwtCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeHelper.GetCurrentTime().AddDays(1),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            _httpContextAccessor.HttpContext?.Response.Cookies.Append("token", token, cookieOptions);
        }
    }
}
