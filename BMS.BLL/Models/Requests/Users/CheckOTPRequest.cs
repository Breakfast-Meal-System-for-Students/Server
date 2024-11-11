using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Requests.Users
{
    public class CheckOTPRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string OTP { get; set; }
    }
}
