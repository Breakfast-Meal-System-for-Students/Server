using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Models.Requests.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.IServices
{
    public interface IUserService
    {
        Task<ServiceActionResult> GetUserByID(Guid userID);
        Task<ServiceActionResult> GetUserByEmail(string email);
        Task<ServiceActionResult> GetTotalUser();
        Task<ServiceActionResult> GetListUser(SearchStaffRequest request);

        Task<ServiceActionResult> DeleteUser(Guid id);
        /*Task<ServiceActionResult> ChangePassword(ChangePWForm userDto);
        Task<ServiceActionResult> UpdateUserProfile(Account user);
        Task<ServiceActionResult> ResetPassWord(ResetPassWordRequest request);
        Task<ServiceActionResult> ResetPasswordAsync(RPFormRequest request);
        
        
        Task<ServiceActionResult> GetAccountsByDateAsync(DateTime date);
        Task<ServiceActionResult> GetAccountCountByDateAsync(DateTime date);*/
    }
}
