﻿using BMS.BLL.Models;
using BMS.BLL.Models.Requests.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.IServices
{
    public interface IAuthService
    {
        Task<ServiceActionResult> LoginAsync(LoginUser userToLoginDTO);
        Task<ServiceActionResult> RegisterAsync(RegisterUser userToRegisterDTO, int role = 3);
        Task<ServiceActionResult> ConfirmEmail(string userId, string token);

        Task<ServiceActionResult> SendOTP(string email);
        Task<ServiceActionResult> CheckOTP(string email, string OTP);
        Task<ServiceActionResult> RegisterStudentAsync(RegisterStudent userToRegisterDTO);
        Task<IList<string>> GetRole(Guid userId);
        Task<ServiceActionResult> RegisterStudentNoMailEduAsync(RegisterStudentNoMailEdu userToRegisterDTO);

    }
}
