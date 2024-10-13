using AutoMapper;
using Azure.Core;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.OpeningHour;
using BMS.BLL.Models.Responses.Admin;
using BMS.BLL.Models.Responses.OpeningHour;
using BMS.BLL.Models.Responses.Users;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Entities;
using BMS.Core.Helpers;
using BMS.DAL;
using Microsoft.Extensions.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services
{
    public class OpeningHoursService : BaseService, IOpeningHoursService
    {
        private readonly IOpeningHoursService _openingHoursService;
        public OpeningHoursService(IUnitOfWork unitOfWork, IMapper mapper, IOpeningHoursService openingHoursService) : base(unitOfWork, mapper)
        {
            _openingHoursService = openingHoursService;
        }

        public async Task<ServiceActionResult> GetOpeningHoursForShop(Guid shopId)
        {
            var openingHours = (await _unitOfWork.OpeningHoursRepository.GetAllAsyncAsQueryable()).Where(x => x.ShopId == shopId);
            var paginationResult = PaginationHelper
            .BuildPaginatedResult<OpeningHours, GetOpeningHoursForShopResonse>(_mapper, openingHours, 7, 1);

            return new ServiceActionResult(true) { Data = paginationResult };
        }

        public async Task<ServiceActionResult> UpdateOpeningHoursForShop(UpdateOpeningHoursRequest request)
        {
            var openingHours = (await _unitOfWork.OpeningHoursRepository.GetAllAsyncAsQueryable()).Where(x => x.ShopId == request.shopId).ToList();
            if (openingHours.Any()) 
            {
                foreach (OpeningHoursRequest openingHour in request.listOpeningHours)
                {
                    //var op = _mapper.Map<OpeningHours>(openingHour);

                    //op.ShopId = request.shopId;
                    openingHours.SingleOrDefault(x => x.day == openingHour.day).Set(openingHour.from_hour, openingHour.to_hour, openingHour.from_minute, openingHour.to_minute);
                }
                await _unitOfWork.OpeningHoursRepository.UpdateRangeAsync(openingHours);
                await _unitOfWork.CommitAsync();
                return new ServiceActionResult(true)
                {
                    Detail = "Update OpeningHoursForShop Successfully"
                };
            }
            return new ServiceActionResult(true)
            {
                Detail = "Shop is not existed or deleted"
            };
        }
    }
}
