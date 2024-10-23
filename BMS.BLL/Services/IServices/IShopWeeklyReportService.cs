using BMS.BLL.Models;
using BMS.BLL.Models.Requests.ShopWeeklyReport;
using BMS.Core.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.IServices
{
    public interface IShopWeeklyReportService
    {
        Task<ServiceActionResult> GetAllShopWeeklyReport(GetShopWeeklyReportRequest request);
        Task<ServiceActionResult> CreateShopWeeklyReport(Guid shopId, byte[] PDFfile);
        Task SaveChange();
    }
}
