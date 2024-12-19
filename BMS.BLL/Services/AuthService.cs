using AutoMapper;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.User;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.BLL.Utilities;
using BMS.Core.Domains.Constants;
using BMS.Core.Domains.Entities;
using BMS.Core.Exceptions;
using BMS.DAL;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ICookieService _cookieService;

        public AuthService(ITokenService tokenService, IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IMapper mapper,
            RoleManager<Role> roleManager,
            IEmailService emailService,
            ICookieService cookieService) : base(unitOfWork, mapper)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _emailService = emailService;
            _cookieService = cookieService;
        }

        public async Task<ServiceActionResult> CheckOTP(string email, string OTP)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ServiceActionResult(false, "User not found");
            }
            var otp = (await _unitOfWork.OTPRepository.GetAllAsyncAsQueryable()).Where(x => x.UserId == user.Id && x.Otp == OTP).FirstOrDefault();
            if (otp == null)
            {
                return new ServiceActionResult(true)
                {
                    Data = false,
                    Detail = "OTP is not true"
                };
            } else
            {
                if (otp.EndDate < DateTimeHelper.GetCurrentTime())
                {
                    return new ServiceActionResult(true)
                    {
                        Data = false,
                        Detail = "OTP is expired"
                    };
                }
                else
                {
                    return new ServiceActionResult(true)
                    {
                        Data = true,
                    };
                }
            }

        }

        public async Task<ServiceActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return new ServiceActionResult(false);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new UserNotFoundException();

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                return new ServiceActionResult(false) { Detail = result.Errors.First().Description };

            return new ServiceActionResult(true, "Email comfirmed");
        }

   

        public async Task<ServiceActionResult> LoginAsync(LoginUser userToLoginDTO)
        {
            var user = await _userManager.FindByNameAsync(userToLoginDTO.Email);
            if (user == null)
                throw new UserNotFoundException($"Invalid user with email {userToLoginDTO.Email}");

            var isEmailActivated = await _userManager.IsEmailConfirmedAsync(user);
            if (!isEmailActivated)
            {
                if (user.CreateDate.AddHours(0.5) < DateTimeHelper.GetCurrentTime())
                {
                    var token1 = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _emailService.SendEmailConfirmationMoblieAsync(user, token1);
                    throw new UnactivatedEmailException($"User with email {userToLoginDTO.Email} unconfirmed. We have sent email to confirm again");
                }
                throw new UnactivatedEmailException($"User with email {userToLoginDTO.Email} unconfirmed.");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, userToLoginDTO.Password, false);
            if (!result.Succeeded)
                throw new InvalidPasswordException($"Invalid password");

            var token = await _tokenService.CreateToken(user);
            _cookieService.SetJwtCookie(token);

            return new ServiceActionResult(true) { Data = new { roles = await _userManager.GetRolesAsync(user), token = token } };
        }

        public async Task<ServiceActionResult> RegisterAsync(RegisterUser userToRegisterDTO, int role = 3)
        {
            var userEntity = _mapper.Map<User>(userToRegisterDTO);

            var result = await _userManager.CreateAsync(userEntity, userToRegisterDTO.Password);
            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                return new ServiceActionResult(false, error.Description);
            }
            string roleName = "";
            switch (role)
            {
                case 0: roleName = UserRoleConstants.USER; break;
                case 1: roleName = UserRoleConstants.ADMIN; break;
                case 2: roleName = UserRoleConstants.STAFF; break;
                case 3: roleName = UserRoleConstants.USER; break;
                case 4: roleName = UserRoleConstants.SHOP; break;
            }

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new Role { Name = roleName });
            }
            var roleResult = await _userManager.AddToRoleAsync(userEntity, roleName);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(userEntity);
                throw new AddRoleException("Can not create account with role");
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(userEntity);
            await _emailService.SendEmailConfirmationMoblieAsync(userEntity, token);

            return new ServiceActionResult(true);
        }


        public async Task<ServiceActionResult> RegisterStudentAsync(RegisterStudent userToRegisterDTO)
        {
            //        // Danh sách các tên miền của các trường đại học
            //        var allowedDomains = new List<string>
            //{
            //    "@truong1.edu.vn",
            //    "@truong2.edu.vn",
            //    "@truong3.edu.vn" // Thêm các tên miền khác tại đây
            //};

            //        // Kiểm tra nếu email không thuộc bất kỳ tên miền nào trong danh sách
            //        if (!allowedDomains.Any(domain => userToRegisterDTO.Email.EndsWith(domain, StringComparison.OrdinalIgnoreCase)))
            //        {
            //            return new ServiceActionResult(false, "Chỉ các email thuộc trường đại học được phép đăng ký.");
            //        }
            if (!userToRegisterDTO.Email.Contains("edu", StringComparison.OrdinalIgnoreCase))
            {
                return new ServiceActionResult(false, "This mail not mail edu");
            }
            var userEntity = _mapper.Map<User>(userToRegisterDTO);
            
            var result = await _userManager.CreateAsync(userEntity, userToRegisterDTO.Password);
            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                return new ServiceActionResult(false, error.Description);
            }
       
               string roleName = UserRoleConstants.STAFF;
        
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new Role { Name = roleName });
            }
            var roleResult = await _userManager.AddToRoleAsync(userEntity, roleName);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(userEntity);
                throw new AddRoleException("Can not create account with role");
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(userEntity);
            await _emailService.SendEmailConfirmationMoblieAsync(userEntity, token);

            return new ServiceActionResult(true);
        }

        public async Task<ServiceActionResult> RegisterStudentNoMailEduAsync(RegisterStudentNoMailEdu userToRegisterDTO)
        {
  
            var userEntity = _mapper.Map<User>(userToRegisterDTO);

            var result = await _userManager.CreateAsync(userEntity, userToRegisterDTO.Password);
            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                return new ServiceActionResult(false, error.Description);
            }

            string roleName = UserRoleConstants.STAFF;

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new Role { Name = roleName });
            }
            var roleResult = await _userManager.AddToRoleAsync(userEntity, roleName);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(userEntity);
                throw new AddRoleException("Can not create account with role");
            }


            //var token = await _userManager.GenerateEmailConfirmationTokenAsync(userEntity);
           // await _emailService.SendEmailConfirmationMoblieAsync(userEntity, token);

            return new ServiceActionResult(true);
        }

        public async Task<ServiceActionResult> SendOTP(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ServiceActionResult(false, "User not found");
            }
            string otp = GenerateOTP(6);
            var dbOTPs = (await _unitOfWork.OTPRepository.GetAllAsyncAsQueryable()).Where(x => x.UserId == user.Id).ToList();
            while(dbOTPs.Any(x => x.Otp == otp && x.EndDate > DateTimeHelper.GetCurrentTime()))
            {
                otp = GenerateOTP(6);
            }
            await _emailService.SendEmailOTP(email, otp);
            OTP o = new OTP()
            {
                Otp = otp,
                EndDate = DateTimeHelper.GetCurrentTime().AddMinutes(1),
                UserId = user.Id,
            };
            await _unitOfWork.OTPRepository.DeleteRangeAsync((dbOTPs.Where(x => x.EndDate < DateTimeHelper.GetCurrentTime())));
            await _unitOfWork.OTPRepository.AddAsync(o);
            return new ServiceActionResult(true)
            {
                Data = otp
            };
        }

        public async Task<IList<string>> GetRole(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new UserNotFoundException($"Invalid user with email {userId}");
            return await _userManager.GetRolesAsync(user);
        }

        private string GenerateOTP(int length)
        {
            string numbers = "1234567890";
            Random random = new Random();
            string otp = string.Empty;

            for (int i = 0; i < length; i++)
            {
                otp += numbers[random.Next(numbers.Length)];
            }

            return otp;
        }
    }
}
