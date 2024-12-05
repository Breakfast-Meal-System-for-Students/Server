using AutoMapper;
using Azure.Core;
using BMS.BLL.Helpers;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Models.Requests.Basic;
using BMS.BLL.Models.Requests.User;
using BMS.BLL.Models.Responses.Shop;
using BMS.BLL.Models.Responses.Users;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.BLL.Utilities;
using BMS.Core.Domains.Constants;
using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using BMS.Core.Exceptions;
using BMS.Core.Helpers;
using BMS.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BMS.BLL.Services
{
    public class StaffService : BaseService, IStaffService
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly RoleManager<Role> _roleManager;

        public StaffService(IUnitOfWork unitOfWork,
            IMapper mapper, UserManager<User> userManager, IEmailService emailService, RoleManager<Role> roleManager) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
            _emailService = emailService;
            _roleManager = roleManager;
        }

        public async Task<ServiceActionResult> AddStaff(CreateStaffRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                bool isInShopRole = roles.Contains(UserRoleConstants.STAFF);
                if (isInShopRole)
                {
                    return new ServiceActionResult(false)
                    {
                        Detail = $"{request.Email} is already used by a Staff in System"
                    };
                }
                else
                {
                    return new ServiceActionResult(false)
                    {
                        Detail = $"{request.Email} is already used in System. Please Input another Email"
                    };
                }
                /*if (!await _roleManager.RoleExistsAsync(UserRoleConstants.STAFF))
                {
                    await _roleManager.CreateAsync(new Role { Name = UserRoleConstants.STAFF });
                }
                var roleResultIn = await _userManager.AddToRoleAsync(user, UserRoleConstants.STAFF);

                if (!roleResultIn.Succeeded)
                {
                    var error = roleResultIn.Errors.First().Description;
                    throw new AddRoleException(error);
                }
                return new ServiceActionResult(true, "Add Role Staff For User Complete");*/
            }
            var staff = _mapper.Map<User>(request);

            try
            {
                var password = AccountCreationHelper.GenerateRandomPassword();
                staff.UserName =  staff.Email;
                await _userManager.CreateAsync(staff, password);
                if (!await _roleManager.RoleExistsAsync(UserRoleConstants.STAFF))
                {
                    await _roleManager.CreateAsync(new Role { Name = UserRoleConstants.STAFF });
                }
                var roleResult = await _userManager.AddToRoleAsync(staff, UserRoleConstants.STAFF);
                if (!roleResult.Succeeded)
                {
                    await _userManager.DeleteAsync(staff);
                    throw new AddRoleException("Can not create account with role");
                }
                await _emailService.SendEmailAsync(request.Email, "YOU HAVE NEW INFORMATION FROM BMS", EmailHelper.GetEmailToSendPWToStaff("thunghiem3340@gmail.com", request.Email, password), true);
                return new ServiceActionResult();
            }
            catch
            {
                throw new BusinessRuleException("Error");
            }
        }

        public async Task<ServiceActionResult> DeleteStaff(Guid id)
        {
            /*var user = await _userManager.FindByIdAsync(id.ToString());
            await _userManager.DeleteAsync(user);*/
            await _unitOfWork.UserRepository.SoftDeleteByIdAsync(id);
            return new ServiceActionResult();
        }

        public async Task<ServiceActionResult> GetListStaff(SearchStaffRequest request)
        {
            IQueryable<User> staffQuery = (await _unitOfWork.UserRepository.GetAllAsyncAsQueryable()).Include(a => a.UserRoles).ThenInclude(b => b.Role).Where(x => x.UserRoles.Any(a => a.Role.Name.Contains(UserRoleConstants.STAFF)));

            /*var canParsed = Enum.TryParse(queryParameters.Status, true, out ShopStatus status);
            if (canParsed)
            {
                applicationQuery = applicationQuery.Where(m => m.Status == status);
            }*/

            if (!string.IsNullOrEmpty(request.Search))
            {
                staffQuery = staffQuery.Where(m => m.Email.Contains(request.Search) || (m.LastName + m.FirstName).Contains(request.Search));
            }

            staffQuery = request.IsDesc ? staffQuery.OrderByDescending(a => a.CreateDate) : staffQuery.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<User, UserLoginResponse>(_mapper, staffQuery, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };
        }

        public async Task<ServiceActionResult> GetStaffById(Guid id)
        {
            var staff = await _unitOfWork.UserRepository.FindAsync(id);
            if (staff != null)
            {
                var roles = await _userManager.GetRolesAsync(staff);
                bool isInShopRole = roles.Contains(UserRoleConstants.STAFF);
                if (!isInShopRole)
                {
                    throw new BusinessRuleException($"{staff.Email} is not a Staff in System");
                }
                var returnStaff = _mapper.Map<UserLoginResponse>(staff);

                return new ServiceActionResult(true) { Data = returnStaff };
            } else
            {
                return new ServiceActionResult(false, "Staff is not exits or deleted");
            }

            
        }

        public async Task<ServiceActionResult> GetStaffByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceActionResult> GetTotalStaff()
        {
            return new ServiceActionResult()
            {
                Data = (await _unitOfWork.UserRepository.GetAllAsyncAsQueryable()).Include(a => a.UserRoles).ThenInclude(b => b.Role).Where(x => x.UserRoles.Any(a => a.Role.Name.Contains(UserRoleConstants.STAFF))).Count()
            };
        }
    }
}
