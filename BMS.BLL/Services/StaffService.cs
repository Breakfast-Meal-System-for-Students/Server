using AutoMapper;
using Azure.Core;
using BMS.BLL.Helpers;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Models.Requests.Basic;
using BMS.BLL.Models.Requests.User;
using BMS.BLL.Models.Responses.Shop;
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
                    throw new BusinessRuleException($"{request.Email} is already used by a Staff in System");
                }
                if (!await _roleManager.RoleExistsAsync(UserRoleConstants.STAFF))
                {
                    await _roleManager.CreateAsync(new Role { Name = UserRoleConstants.STAFF });
                }
                var roleResultIn = await _userManager.AddToRoleAsync(user, UserRoleConstants.STAFF);

                if (!roleResultIn.Succeeded)
                {
                    var error = roleResultIn.Errors.First().Description;
                    throw new AddRoleException(error);
                }
                return new ServiceActionResult(true, "Add Role Staff For User Complete");
            }
            var staff = _mapper.Map<User>(request);

            try
            {
                var password = AccountCreationHelper.GenerateRandomPassword();
                await _userManager.CreateAsync(staff, password);
                await _emailService.SendEmailAsync(request.Email, "YOU HAVE NEW INFORMATION FROM BMS", EmailHelper.GetAcceptedEmailBody("thunghiem3340@gmail.com", request.Email, password), true);
                return new ServiceActionResult();
            }
            catch
            {
                throw new BusinessRuleException("Error");
            }
        }

        public async Task<ServiceActionResult> DeleteStaff(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            await _userManager.DeleteAsync(user);
            return new ServiceActionResult();
        }

        public async Task<ServiceActionResult> GetListStaff(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceActionResult> GetStaffByMail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceActionResult> GetStaffByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceActionResult> GetTotalStaff()
        {
            throw new NotImplementedException();
        }
    }
}
