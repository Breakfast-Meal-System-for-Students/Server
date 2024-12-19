using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.StudentApplication
{
    public class UpdateStudentApplicationRequest
    {
        public Guid UserId { get; set; }
        public string StudentId { get; set; } = null!;
        public string ImageCardStudent { get; set; } = null!;


        public Guid UniversityId { get; set; }

        // Implement ISoftDelete properties
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
    }
}
