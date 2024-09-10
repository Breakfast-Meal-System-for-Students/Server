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
        Task<ServiceActionResult> GetListStaff(PagingRequest request);
        Task<ServiceActionResult> GetStaffByMail(string email);
        Task<ServiceActionResult> GetStaffByName(string name);
        Task<ServiceActionResult> GetTotalStaff();
        Task<ServiceActionResult> AddStaff(CreateStaffRequest name);
        Task<ServiceActionResult> DeleteStaff(Guid id);
    }
}
