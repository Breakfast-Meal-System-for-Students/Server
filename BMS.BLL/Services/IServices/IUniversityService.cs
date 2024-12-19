using BMS.BLL.Models.Requests.Package;
using BMS.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.BLL.Models.Requests.University;

namespace BMS.BLL.Services.IServices
{

    public interface IUniversityService
    {
        Task<ServiceActionResult> GetAllUniversity(UniversityRequest queryParameters);
        Task<ServiceActionResult> AddUniversity(CreateUniversityRequest request);

        Task<ServiceActionResult> UpdateUniversity(Guid id, UpdateUniversityRequest request);
        Task<ServiceActionResult> DeleteUniversity(Guid id);
        Task<ServiceActionResult> GetUniversity(Guid id);
      
    }
}
