using AutoMapper;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Users;
using BMS.BLL.Models.Responses.Users;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Entities;
using BMS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services
{
    public class UserService : BaseService, IUserService
    {

        public UserService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public Task<ServiceActionResult> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceActionResult> GetUserByID(Guid userID)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceActionResult> Login(UserLoginRequest request)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(a => a.Email == request.Email);
            if (user != null) 
            {
                if(VerifyPassword(user.Password, request.Password))
                {
                    var response = _mapper.Map<UserLoginResponse>(user);
                    //response.Role = _unitOfWork.
                    return new ServiceActionResult() { Data =  response};
                }
            }
            return new ServiceActionResult() { Data = null };
        }

        public async Task<ServiceActionResult> RegisterUser(UserRegisterRequest request)
        {
                var user = await _unitOfWork.UserRepository.GetAsync(a => a.Email == request.Email);
                if (user == null)
                {
                    request.Password = HashPassword(request.Password);
                    user = _mapper.Map<User>(request);
                    await _unitOfWork.UserRepository.AddAsync(user);
                    var response = _mapper.Map<UserLoginResponse>(user);
                    return new ServiceActionResult() { Data = response };
                }
                else
                {
                    return new ServiceActionResult(true, "Email Already Exit");
                }
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string hashedPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
