using BMS.BLL.Models.Requests.StudentApplication;
using BMS.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.IServices
{

    public interface IStudentApplicationService
    {
        Task<ServiceActionResult> GetAllStudentApplication(StudentApplicationRequest queryParameters);
        void AddStudentApplication(CreateStudentApplicationRequest request);

        Task<ServiceActionResult> UpdateStudentApplication(Guid id, UpdateStudentApplicationRequest request);
        Task<ServiceActionResult> DeleteStudentApplication(Guid id);
        Task<ServiceActionResult> GetStudentApplication(Guid id);

    }
}
