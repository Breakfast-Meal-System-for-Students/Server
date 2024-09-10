using AutoMapper;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.User;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
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
                throw new UnactivatedEmailException($"User with email {userToLoginDTO.Email} unconfirmed.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, userToLoginDTO.Password, false);
            if (!result.Succeeded)
                throw new InvalidPasswordException($"Invalid password");

            var token = await _tokenService.CreateToken(user);
            _cookieService.SetJwtCookie(token);

            return new ServiceActionResult(true) { Data = new { roles = await _userManager.GetRolesAsync(user), token = token } };
        }

        public async Task<ServiceActionResult> RegisterAsync(RegisterUser userToRegisterDTO)
        {
            var userEntity = _mapper.Map<User>(userToRegisterDTO);

            var result = await _userManager.CreateAsync(userEntity, userToRegisterDTO.Password);
            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                return new ServiceActionResult(false, error.Description);
            }

            if (!await _roleManager.RoleExistsAsync(UserRoleConstants.ADMIN))
            {
                await _roleManager.CreateAsync(new Role { Name = UserRoleConstants.ADMIN });
            }
            var roleResult = await _userManager.AddToRoleAsync(userEntity, UserRoleConstants.ADMIN);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(userEntity);
                throw new AddRoleException("Can not create account with role");
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(userEntity);
            await _emailService.SendEmailConfirmationAsync(userEntity, token);

            return new ServiceActionResult(true);
        }


    }
}
