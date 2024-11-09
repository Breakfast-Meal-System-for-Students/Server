using AutoMapper;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Favourite;
using BMS.BLL.Models.Responses.Favourite;
using BMS.BLL.Models.Responses.Users;
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
    public class FavouriteService : BaseService, IFavouriteService
    {
        public FavouriteService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<ServiceActionResult> AddFavourite(Guid userId, Guid shopId)
        {
            var favourite = (await _unitOfWork.FavoriteRepository.GetAllAsyncAsQueryable()).Where(x => x.UserId == userId && x.ShopId == shopId).Include(a => a.Shop).FirstOrDefault();
            if (favourite != null) 
            {
                return new ServiceActionResult(false) { Detail = "Shop is Favourited by you" };
            }else
            {
                Favourite newfavourite = new Favourite()
                {
                    UserId = userId,
                    ShopId = shopId
                };
                await _unitOfWork.FavoriteRepository.AddAsync(newfavourite);
                return new ServiceActionResult(true)
                {
                    Data = newfavourite
                };
            }

        }

        public async Task<ServiceActionResult> DeleteFavourite(Guid favouriteId)
        {
            await _unitOfWork.FavoriteRepository.DeleteAsync(favouriteId);
            return new ServiceActionResult(true, "Delete Successfully");
        }

        public async Task<ServiceActionResult> GetFavourite(Guid userId, Guid shopId)
        {
            var favourite = (await _unitOfWork.FavoriteRepository.GetAllAsyncAsQueryable()).Where(x => x.UserId == userId && x.ShopId == shopId).Include(a => a.Shop).FirstOrDefault();
            var returnFavourite = _mapper.Map<FavouriteResponse>(favourite);

            return new ServiceActionResult(true) { Data = returnFavourite };
        }

        public async Task<ServiceActionResult> GetFavouriteList(GetFavouriteList request, Guid userId)
        {
            IQueryable<Favourite> favouriteList = (await _unitOfWork.FavoriteRepository.GetAllAsyncAsQueryable()).Where(x => x.UserId == userId).Include(a => a.Shop);
            if (!string.IsNullOrEmpty(request.ShopName))
            {
                favouriteList = favouriteList.Where(x => x.Shop.Name.Contains(request.ShopName));
            }
            favouriteList = request.IsDesc ? favouriteList.OrderByDescending(a => a.CreateDate) : favouriteList.OrderBy(a => a.CreateDate);
            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Favourite, FavouriteResponse>(_mapper, favouriteList, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };
        }
    }
}
