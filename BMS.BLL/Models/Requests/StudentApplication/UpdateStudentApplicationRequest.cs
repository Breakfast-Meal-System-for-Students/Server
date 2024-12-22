using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.StudentApplication
{
    public class UpdateStudentApplicationRequest
    {
        public Guid Id { get; set; }
        public StudentStatus? StatusStudent { get; set; }

        public string Message { get; set; } = "";
       
    }
}
