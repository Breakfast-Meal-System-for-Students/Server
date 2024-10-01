using AutoMapper;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Models.Requests.Users;
using BMS.BLL.Models.Responses.Users;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Constants;
using BMS.Core.Domains.Entities;
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

namespace BMS.BLL.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly UserManager<User> _userManager;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
        }

        public async Task<ServiceActionResult> DeleteUser(Guid id)
        {
            await _unitOfWork.UserRepository.SoftDeleteByIdAsync(id);
            return new ServiceActionResult(true, "Delete Successfully");
        }

        public async Task<ServiceActionResult> GetListUser(SearchStaffRequest request)
        {
            IQueryable<User> userQuery = (await _unitOfWork.UserRepository.GetAllAsyncAsQueryable()).Include(a => a.UserRoles).ThenInclude(b => b.Role).Where(x => x.UserRoles.Any(a => a.Role.Name.Contains(UserRoleConstants.USER)));

            /*var canParsed = Enum.TryParse(queryParameters.Status, true, out ShopStatus status);
            if (canParsed)
            {
                applicationQuery = applicationQuery.Where(m => m.Status == status);
            }*/

            if (!string.IsNullOrEmpty(request.Search))
            {
                userQuery = userQuery.Where(m => m.Email.Contains(request.Search) || (m.LastName + m.FirstName).Contains(request.Search));
            }

            userQuery = request.IsDesc ? userQuery.OrderByDescending(a => a.CreateDate) : userQuery.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<User, UserLoginResponse>(_mapper, userQuery, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };
        }

        public async Task<ServiceActionResult> GetTotalUser()
        {
            return new ServiceActionResult()
            {
                Data = (await _unitOfWork.UserRepository.GetAllAsyncAsQueryable()).Include(a => a.UserRoles).ThenInclude(b => b.Role).Where(x => x.UserRoles.Any(a => a.Role.Name.Contains(UserRoleConstants.USER))).Count()
            };
        }

        public async Task<ServiceActionResult> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                bool isInShopRole = roles.Contains(UserRoleConstants.USER);
                if (!isInShopRole)
                {
                    throw new BusinessRuleException($"{user.Email} is not a User in System");
                }
                var returnStaff = _mapper.Map<UserLoginResponse>(user);

                return new ServiceActionResult(true) { Data = returnStaff };
            }
            else
            {
                return new ServiceActionResult(false, "Staff is not exits or deleted");
            }
        }

        public async Task<ServiceActionResult> GetUserByID(Guid userID)
        {
            var user = await _unitOfWork.UserRepository.FindAsync(userID);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                bool isInShopRole = roles.Contains(UserRoleConstants.USER);
                if (!isInShopRole)
                {
                    throw new BusinessRuleException($"{user.Email} is not a User in System");
                }
                var returnStaff = _mapper.Map<UserLoginResponse>(user);

                return new ServiceActionResult(true) { Data = returnStaff };
            }
            else
            {
                return new ServiceActionResult(false, "User is not exits or deleted");
            }
        }
    }
}
