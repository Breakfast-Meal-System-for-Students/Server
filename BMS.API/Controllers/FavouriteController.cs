using BMS.API.Controllers.Base;
using BMS.API.Hub;
using BMS.BLL.Models;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services;
using BMS.BLL.Services.IServices;
using Microsoft.AspNetCore.SignalR;
using BMS.BLL.Models.Requests.Feedbacks;
using Microsoft.AspNetCore.Mvc;
using BMS.BLL.Models.Requests.Basic;
using BMS.BLL.Models.Requests.Favourite;
using Microsoft.AspNetCore.Authorization;
using BMS.Core.Domains.Constants;

namespace BMS.API.Controllers
{
    public class FavouriteController : BaseApiController
    {
        private readonly IFavouriteService _favoriteService;
        private readonly IUserClaimsService _userClaimsService;
        private UserClaims _userClaims;
        public FavouriteController(IFavouriteService favouriteService, IUserClaimsService userClaimsService)
        {
            _baseService = (BaseService)favouriteService;
            _favoriteService = favouriteService;
            _userClaimsService = userClaimsService;
            _userClaims = userClaimsService.GetUserClaims();
        }

        [HttpGet("GetFavouriteList")]
        [Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> GetFavouriteList([FromQuery] GetFavouriteList request)
        {
            return await ExecuteServiceLogic(
                               async () => await _favoriteService.GetFavouriteList(request, _userClaims.UserId).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpGet("GetFavourite")]
        [Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> GetFavourite(Guid shopId)
        {
            return await ExecuteServiceLogic(
                               async () => await _favoriteService.GetFavourite(_userClaims.UserId, shopId).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpPost("AddFavourite")]
        [Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> AddFavourite([FromForm] Guid shopId)
        {
            return await ExecuteServiceLogic(
                               async () => await _favoriteService.AddFavourite(_userClaims.UserId, shopId).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }

        [HttpGet("DeleteFavourite/{favouriteId}")]
        [Authorize(Roles = UserRoleConstants.USER)]
        public async Task<IActionResult> DeleteFavourite(Guid favouriteId)
        {
            return await ExecuteServiceLogic(
                               async () => await _favoriteService.DeleteFavourite(favouriteId).ConfigureAwait(false)
                                          ).ConfigureAwait(false);
        }
    }
}
