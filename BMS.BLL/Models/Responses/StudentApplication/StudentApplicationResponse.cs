using BMS.BLL.Models.Responses.University;
using BMS.BLL.Models.Responses.Users;
using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using BMS.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.StudentApplication
{
    public class StudentApplicationResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string StudentId { get; set; } = null!;

        public string NameUniversity { get; set; } = null!;
        public string ImageCardStudent { get; set; } = null!;

        public bool isOpenToday { get; set; }
        // Relationships
        public Guid UniversityId { get; set; }
        public UniversityResponse University { get; set; } = null!;

        public StudentStatus? StatusStudent { get; set; }

        public UserResponse? User { get; set; } = null!;

    }
}
