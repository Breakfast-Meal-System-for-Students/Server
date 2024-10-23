using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.ShopWeeklyReport
{
    public class ShopWeeklyReportResponse
    {
        public Guid ShopId { get; set; }
        public string? ShopName {  get; set; }
        public string? ShopImage { get; set; }
        public byte[]? Report {  get; set; }
        public DateTime DateReport { get; set; }
    }
}
