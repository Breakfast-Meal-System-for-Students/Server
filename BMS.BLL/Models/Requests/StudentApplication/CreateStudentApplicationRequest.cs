using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.StudentApplication
{
    public class CreateStudentApplicationRequest
    {
        public Guid UserId { get; set; }
        public string StudentId { get; set; } = null!;
        public string ImageCardStudent { get; set; } = null!;

        public string Email { get; set; } = null!;
        public StudentStatus? StatusStudent { get; set; }
        public Guid UniversityId { get; set; }
    }
}
