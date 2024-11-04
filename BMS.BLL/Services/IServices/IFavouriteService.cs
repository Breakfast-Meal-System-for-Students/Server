using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Favourite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.IServices
{
    public interface IFavouriteService
    {
        Task<ServiceActionResult> GetFavouriteList(GetFavouriteList request, Guid userId);
        Task<ServiceActionResult> AddFavourite(Guid userId, Guid shopId);
        Task<ServiceActionResult> DeleteFavourite(Guid favouriteId);
        Task<ServiceActionResult> GetFavourite(Guid userId, Guid shopId);
    }
}
