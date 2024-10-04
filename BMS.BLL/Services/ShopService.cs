using AutoMapper;
using BMS.BLL.Helpers;
using BMS.BLL.Models.Requests.Shop;
using BMS.BLL.Models.Requests.User;
using BMS.BLL.Models.Responses.Shop;
using BMS.BLL.Models;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.BLL.Models.Requests.Shop;
using BMS.BLL.Models.Responses.Shop;

namespace BMS.BLL.Services
{
 
    public class ShopService : BaseService, IShopService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ShopService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceActionResult> GetAllShop(ShopRequest queryParameters)
        {

            IQueryable<Shop> ShopQueryable = (await _unitOfWork.ShopRepository.GetAllAsyncAsQueryable());



            if (!string.IsNullOrEmpty(queryParameters.Search))
            {
                ShopQueryable = ShopQueryable.Where(m => m.Name.Contains(queryParameters.Search));
            }

            ShopQueryable = queryParameters.IsDesc ? ShopQueryable.OrderByDescending(a => a.CreateDate) : ShopQueryable.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Shop, ShopResponse>(_mapper, ShopQueryable, queryParameters.PageSize, queryParameters.PageIndex);



            return new ServiceActionResult() { Data = paginationResult };
        }
   

        public async Task<ServiceActionResult> UpdateShop(Guid id, UpdateShopRequest request)
        {


            var Shop = await _unitOfWork.ShopRepository.FindAsync(id) ?? throw new ArgumentNullException("Shop is not exist");


            Shop.LastUpdateDate = DateTime.UtcNow;
            Shop.Name = request.Name;
            Shop.Description = request.Description;
            Shop.Address = request.Address;
            await _unitOfWork.CommitAsync();

            return new ServiceActionResult(true) { Data = Shop };
        }


        public async Task<ServiceActionResult> GetShop(Guid id)
        {
            var Shop = await _unitOfWork.ShopRepository
         .FindAsync(c => c.Id == id)
         ?? throw new ArgumentNullException("Shop does not exist or has been deleted");
            var returnShop = _mapper.Map<ShopResponse>(Shop);

            return new ServiceActionResult(true) { Data = returnShop };
        }
        public async Task<ServiceActionResult> DeleteShop(Guid id)
        {
            await _unitOfWork.ShopRepository.SoftDeleteByIdAsync(id);
            return new ServiceActionResult();
        }

    }
}
