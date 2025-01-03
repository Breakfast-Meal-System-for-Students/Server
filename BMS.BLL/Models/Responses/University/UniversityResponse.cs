using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.University
{
    public class UniversityResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!; // University name
        public string Address { get; set; } = null!; // University address
        public string EndMail { get; set; } = null!; // Email domain for university students

        public double? Lng { get; set; } // Longitude of the university's location
        public double? Lat { get; set; } // Latitude of the university's location
        public string IdStudentFormat { get; set; } = null!; // Format of student IDs
        public string Abbreviation { get; set; } = null!; // Short form of the university name
    }
}
