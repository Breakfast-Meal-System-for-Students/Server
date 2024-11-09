using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.IServices
{
    public interface IAccountService
    {
        Task<ServiceActionResult> GetDetails(Guid id);

        Task<ServiceActionResult> UpdateDetails(UpdateUserRequest request, Guid userId);
        Task<ServiceActionResult> UpdateAvatar(IFormFile newAvatar, Guid userId);
        Task<ServiceActionResult> UpdatePassword(UpdatePasswordRequest request, Guid userId);
        Task<ServiceActionResult> ResetPassword(string email, string newPassword);
    }
}
