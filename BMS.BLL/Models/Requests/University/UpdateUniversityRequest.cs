using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.University
{
    public class UpdateUniversityRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!; // University name
        public string Address { get; set; } = null!; // University address
        public string EndMail { get; set; } = null!; // Email domain for university students

        public string IdStudentFormat { get; set; } = null!; // Format of student IDs
        public string Abbreviation { get; set; } = null!; // Short form of the university name
    }

}
