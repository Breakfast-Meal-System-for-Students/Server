using AutoMapper;
using BMS.BLL.Models.Requests.Category;
using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Models.Requests.Feedbacks;
using BMS.BLL.Models.Requests.Shop;
using BMS.BLL.Models.Requests.User;
using BMS.BLL.Models.Requests.Users;
using BMS.BLL.Models.Responses.Category;
using BMS.BLL.Models.Responses.Feedbacks;
using BMS.BLL.Models.Responses.Shop;
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


                CreateMap<RegisterUser, User>()
                   .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                   .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            


                CreateMap<User, LoginUser>();


                CreateMap<User, UserLoginResponse>();
                CreateMap<UserRegisterRequest, User>();
                CreateMap<CreateStaffRequest, User>();


                #endregion
                #region shop  

                CreateMap<CreateShopApplicationRequest, Shop>();

                CreateMap<Shop, ShopApplicationResponse>();

                #endregion

                #region feedback
                CreateMap<Feedback, FeedbackResponse>();

                CreateMap<FeedbackRequest, Feedback>();

                CreateMap<Feedback, FeedbackForStaffResponse>()
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User!.FirstName))
                    .ForMember(dest => dest.UserPic, opt => opt.MapFrom(src => src.User!.Avatar))
                    .ForMember(dest => dest.ShopName, opt => opt.MapFrom(src => src.Shop!.Name))
                    .ForMember(dest => dest.Shoppic, opt => opt.MapFrom(src => src.Shop!.Image));
                #endregion

                #region category
                CreateMap<Category, UpdateCategoryRequest>()
                 .ForMember(dest => dest.Image, opt => opt.Ignore());
                CreateMap<CreateCategoryRequest, Category>()
                .ForMember(dest => dest.Image, opt => opt.Ignore()); 

                CreateMap<Category, CategoryResponse>();
                #endregion
            }
        }
    }
}
