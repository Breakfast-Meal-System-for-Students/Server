using BMS.BLL.Models.Requests.Basic;
using BMS.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.BLL.Models.Requests.Admin;

namespace BMS.BLL.Services.IServices
{
    public interface IStaffService
    {
        Task<ServiceActionResult> GetListStaff(SearchStaffRequest request);
        Task<ServiceActionResult> GetStaffById(Guid id);
        Task<ServiceActionResult> GetStaffByName(string name);
        Task<ServiceActionResult> GetTotalStaff();
        Task<ServiceActionResult> AddStaff(CreateStaffRequest request);
        Task<ServiceActionResult> DeleteStaff(Guid id);
    }
}
