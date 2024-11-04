using AutoMapper;
using Azure.Core;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Users;
using BMS.BLL.Models.Responses.Roles;
using BMS.BLL.Models.Responses.Users;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Entities;
using BMS.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services
{
    public class AccountService :BaseService,IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ITokenService _tokenService;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper,
            IFileStorageService fileStorageService,
            UserManager<User> userManager,
            RoleManager<Role> roleManager, ITokenService tokenService) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

       

        public async Task<ServiceActionResult> UpdateAvatar(IFormFile newAvatar, Guid userId)
        {
            var user = await _unitOfWork.UserRepository.FindAsync(userId);
            ArgumentNullException.ThrowIfNull(nameof(user));

            var imageUrl = await _fileStorageService.UploadFileBlobAsync(newAvatar);
            user.Avatar = imageUrl;

            await _unitOfWork.CommitAsync();
            return new ServiceActionResult() { Data = imageUrl };
        }

   

     
        public async Task<ServiceActionResult> GetDetails(Guid id)
        {
            var entity = (await _unitOfWork.UserRepository.GetAllAsyncAsQueryable())
                .Where(u => u.Id == id)
                .Include(u => u.UserRoles)!.ThenInclude(ur => ur.Role).FirstOrDefault()
                ?? throw new NullReferenceException("User are not found");

            var returnUser = _mapper.Map<UserResponse>(entity);
           var role = await _userManager.GetRolesAsync(entity);
            returnUser.Role = role;
         //  returnUser.UserRoles = entity.UserRoles.Select(ur => ur.Role).Select(r => new RoleResponse { Name = r.Name }).ToList();

            return new ServiceActionResult() { Data = returnUser };
        }

        public async Task<ServiceActionResult> UpdateDetails(UpdateUserRequest request, Guid userId)
        {
            var user = await _unitOfWork.UserRepository.FindAsync(userId);
            ArgumentNullException.ThrowIfNull(nameof(user));

            _mapper.Map(request, user);

            await _unitOfWork.CommitAsync();
            return new ServiceActionResult();
        }

        public async Task<ServiceActionResult> UpdatePassword(UpdatePasswordRequest request, Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new ServiceActionResult(false, "User not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                return new ServiceActionResult(false, error.Description);
            }

            return new ServiceActionResult();
        }

        public async Task<ServiceActionResult> ResetPassword(Guid userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new ServiceActionResult(false, "User not found");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                return new ServiceActionResult(false, error.Description);
            }

            return new ServiceActionResult(true)
            {
                Detail = "Reset Password Successfully"
            };
        }
    }
}
