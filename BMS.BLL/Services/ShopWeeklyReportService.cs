using AutoMapper;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.ShopWeeklyReport;
using BMS.BLL.Models.Responses.Admin;
using BMS.BLL.Models.Responses.ShopWeeklyReport;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Entities;
using BMS.Core.Helpers;
using BMS.DAL;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services
{
    public class ShopWeeklyReportService : BaseService,IShopWeeklyReportService
    {
        public ShopWeeklyReportService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        public async Task<ServiceActionResult> CreateShopWeeklyReport(Guid shopId, byte[] PDFfile)
        {
            ShopWeeklyReport shopWeeklyReport = new ShopWeeklyReport()
            {
                ReportData = PDFfile,
                ShopId = shopId,
            };
            await _unitOfWork.ShopWeeklyReportRepository.AddAsync(shopWeeklyReport);
            return new ServiceActionResult();
        }

        public async Task<ServiceActionResult> GetAllShopWeeklyReport(GetShopWeeklyReportRequest request)
        {
            IQueryable<ShopWeeklyReport> reports = (await _unitOfWork.ShopWeeklyReportRepository.GetAllAsyncAsQueryable()).Include(x => x.Shop);

            if (!String.IsNullOrEmpty(request.ShopId.ToString()))
            {
                reports = reports.Where(x => x.ShopId == request.ShopId);
            } else if (!String.IsNullOrEmpty(request.ShopName))
            {
                reports = reports.Where(x => x.Shop.Name.Contains(request.ShopName));
            }

            if (request.Year != 0)
            {
                if (request.Month != 0)
                {
                    if (request.Day != 0)
                    {
                        reports = reports.Where(x => x.CreateDate.Year == request.Year && x.CreateDate.Month == request.Month && x.CreateDate.Day == request.Day);
                    }
                    else
                    {
                        reports = reports.Where(x => x.CreateDate.Year == request.Year && x.CreateDate.Month == request.Month);
                    }
                }
                else
                {
                    reports = reports.Where(x => x.CreateDate.Year == request.Year);
                }
            }

            reports = request.IsDesc ? reports.OrderByDescending(a => a.CreateDate) : reports.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<ShopWeeklyReport, ShopWeeklyReportResponse>(_mapper, reports, request.PageSize, request.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };
        }

        public async Task SaveChange()
        {
            await _unitOfWork.CommitAsync();
        }
    }
}
