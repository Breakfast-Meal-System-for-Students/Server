﻿using AutoMapper;
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
using Microsoft.EntityFrameworkCore;
using BMS.BLL.Models.Responses.Map;

namespace BMS.BLL.Services
{
 
    public class ShopService : BaseService, IShopService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly IGoogleMapService _googleMapService;
        public ShopService(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService, IGoogleMapService googleMapService) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
            _googleMapService = googleMapService;
        }

        public async Task<ServiceActionResult> GetAllShop(ShopRequest queryParameters)
        {

            IQueryable<Shop> ShopQueryable = (await _unitOfWork.ShopRepository.GetAllAsyncAsQueryable()).Include(a => a.Package_Shop).ThenInclude(b => b.Package);



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
            Shop.PhoneNumber = request.Phone;
            Location location = await _googleMapService.GetCoordinatesFromAddress(request.Address);
            if (location == null)
            {
                throw new BusinessRuleException($"Address invalid");
            }
            Shop.lat = location.Lat;
            Shop.lng = location.Lng;
            if (request.Image != null)
            {
                var imageUrl = await _fileStorageService.UploadFileBlobAsync((Microsoft.AspNetCore.Http.IFormFile)request.Image);
                Shop.Image = imageUrl;
            }
            await _unitOfWork.ShopRepository.UpdateAsync(Shop);
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

        public async Task<List<Shop>> GetAllShopToRevenue(DateTime startDate, DateTime endDate)
        {
            return (await _unitOfWork.ShopRepository.GetAllAsyncAsQueryable())
                    .Include(s => s.Orders)
                    .ThenInclude(o => o.Transactions)
                    .Where(s => s.Orders.Any(o => o.CreateDate >= startDate && o.CreateDate <= endDate && o.Transactions.Any(t => t.Status == TransactionStatus.PAID && (t.Method == TransactionMethod.VnPay.ToString() || t.Method == TransactionMethod.PayOs.ToString()))))
                    .ToList();
        }

        public async Task<ServiceActionResult> GetAllShopForMobile(ShopRequest request)
        {
            IQueryable<Shop> ShopQueryable = (await _unitOfWork.ShopRepository.GetAllAsyncAsQueryable()).Include(a => a.Package_Shop).ThenInclude(b => b.Package)
                                        .Where(x => x.Status == ShopStatus.ACCEPTED && (x.Package_Shop.Any()
                                        ? x.Package_Shop.Max(x => x.Package != null ? x.CreateDate.AddDays(x.Package.Duration) : DateTime.MinValue)
                                        : DateTime.MinValue) > DateTime.Now);

            if (!string.IsNullOrEmpty(request.Search))
            {
                ShopQueryable = ShopQueryable.Where(m => m.Name.Contains(request.Search));
            }

            ShopQueryable = request.IsDesc ? ShopQueryable.OrderByDescending(a => a.CreateDate) : ShopQueryable.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Shop, ShopResponse>(_mapper, ShopQueryable, request.PageSize, request.PageIndex);

            return new ServiceActionResult() { Data = paginationResult };
        }
    }
}
