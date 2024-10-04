using AutoMapper;
using Azure.Core;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Basic;
using BMS.BLL.Models.Responses.Cart;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Entities;
using BMS.Core.Helpers;
using BMS.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services
{
    public class CartGroupUserService : BaseService, ICartGroupUserService
    {
        public CartGroupUserService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<ServiceActionResult> GetAllUserInGroupCart(Guid cartId, PagingRequest request)
        {
            var cartGroupUser = (await _unitOfWork.CartGroupUserRepository.GetAllAsyncAsQueryable()).Where(x => x.CartId == cartId).Include(y => y.CartDetails).Include(z => z.User);
            var paginationResult = PaginationHelper
            .BuildPaginatedResult<CartGroupUser, CartGroupUserResponse2>(_mapper, cartGroupUser, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };
        }
    }
}
