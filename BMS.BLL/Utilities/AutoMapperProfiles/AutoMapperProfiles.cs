using AutoMapper;
using BMS.BLL.Models.Requests.Category;
using BMS.BLL.Models.Requests.Admin;
using BMS.BLL.Models.Requests.Feedbacks;
using BMS.BLL.Models.Requests.Shop;
using BMS.BLL.Models.Requests.User;
using BMS.BLL.Models.Requests.Users;
using BMS.BLL.Models.Responses.Admin;
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
using BMS.BLL.Models.Responses.Roles;
using BMS.BLL.Models.Requests.Product;
using BMS.BLL.Models.Responses.Product;
using BMS.BLL.Models.Requests.RegisterCategory;
using BMS.BLL.Models.Responses.RegisterCategory;
using BMS.BLL.Models.Requests.Coupon;
using BMS.BLL.Models.Responses.Coupon;

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


                CreateMap<UpdateUserRequest, User>();


                CreateMap<User, UserResponse>();
                CreateMap<User, LoginUser>();


                CreateMap<User, UserLoginResponse>();
                CreateMap<UserRegisterRequest, User>();
                CreateMap<CreateStaffRequest, User>();
                #region Role
                CreateMap<UserRole, RoleResponse>()
                .ForMember(dest => dest.Name, src => src.MapFrom(opt => opt.Role.Name));
                #endregion

                #endregion
                #region shop  

                CreateMap<CreateShopApplicationRequest, Shop>();

                CreateMap<Shop, ShopApplicationResponse>();

                #endregion
                #region order
                CreateMap<Order, OrderResponse>()
                    .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
                #endregion

                #region transaction
                CreateMap<Transaction, TransactionResponse>()
                     .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order));
                /*CreateMap<Shop, TopResponse>()
                     .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Order));*/
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
                #region product  

                CreateMap<Product, UpdateProductRequest>()
                 .ForMember(dest => dest.Images, opt => opt.Ignore());
                CreateMap<CreateProductRequest, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());

                CreateMap<Product, ProductResponse>();

                #endregion
                #region registerCategory  

                CreateMap<CreateRegisterCategoryRequest,RegisterCategory >();

                CreateMap<RegisterCategory, RegisterCategoryResponse>();
                #endregion
                #region coupon
                CreateMap<Coupon, UpdateCouponRequest>();
                CreateMap<CreateCouponRequest, Coupon>();

                CreateMap<Coupon, CouponResponse>();
                #endregion
            }
        }
    }
}
