using BMS.BLL.Models.Requests.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Validators
{
    public class UserToLoginDTOValidator : AbstractValidator<LoginUser>
    {
        public UserToLoginDTOValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
