using AutoMapper;
using BMS.BLL.Models.Requests.Feedbacks;
using BMS.BLL.Models.Requests.Users;
using BMS.BLL.Models.Responses.Feedbacks;
using BMS.BLL.Models.Responses.Users;
using BMS.Core.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Utilities.AutoMapperProfiles
{
    public static class AutoMapperProfiles
    {
        public class AutoMapperProfile : Profile
        {
            public AutoMapperProfile()
            {
                #region user

                #endregion


                #region feedback
                CreateMap<Feedback, FeedbackResponse>();

                CreateMap<FeedbackRequest, Feedback>();

                CreateMap<User, UserLoginResponse>();
                CreateMap<UserRegisterRequest, User>();
                #endregion


            }
        }
    }
}
