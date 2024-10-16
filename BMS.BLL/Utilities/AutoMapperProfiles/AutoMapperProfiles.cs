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
using BMS.BLL.Models.Requests.Package;
using BMS.BLL.Models.Responses.Package;
using BMS.BLL.Models.Responses.Cart;
using BMS.BLL.Models.Requests.Cart;
using System.Reflection;
using BMS.BLL.Models.Responses.Image;
using BMS.BLL.Models.Responses.Notification;
using BMS.BLL.Models.Responses.ShopWeeklyReport;
using BMS.BLL.Models.Responses.OpeningHour;
using BMS.BLL.Models.Requests.OpeningHour;


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
                CreateMap<Shop, ShopRequest>();
                CreateMap<Shop, ShopResponse>();

                #endregion
                #region order
                CreateMap<OrderItem, OrderItemResponse>()
                    .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                    .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.Product.Images));
                CreateMap<Order, OrderResponse>()
                    .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                    .ForMember(dest => dest.QRCode, opt => opt.MapFrom(src => src.QRCode))
                    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Customer.FirstName))
                    .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Customer.LastName))
                    .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Customer.Avatar))
                    .ForMember(dest => dest.ShopName , opt => opt.MapFrom(src => src.Shop.Name))
                    .ForMember(dest => dest.ShopImage, opt => opt.MapFrom(src => src.Shop.Image));
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
                CreateMap<RegisterCategory, CategoryResponse>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId))
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Category.Name))
                 .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Category.Description))
                 .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Category.Image));
                CreateMap<Category, CategoryResponse>();
                #endregion
                #region product  

                CreateMap<Product, UpdateProductRequest>()
                 .ForMember(dest => dest.Images, opt => opt.Ignore());
                CreateMap<CreateProductRequest, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());

                CreateMap<Product, ProductResponse>()
                    .ForMember(dest => dest.Categorys, opt => opt.MapFrom(src => src.RegisterCategorys));
                #endregion
                #region cart
                CreateMap<CartGroupUser, CartGroupUserResponse>();
                CreateMap<CartDetail, CartDetailResponse>()
                    .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Product!.Images))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product!.Name));
                CreateMap<Cart, CartResponse>()
                    .ForMember(dest => dest.CartDetails, opt => opt.MapFrom(src => src.CartDetails));
                CreateMap<CartDetailRequest, CartDetail>();
                CreateMap<CartGroupUser, CartGroupUserResponse2>();
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
                #region package
                CreateMap<Package, UpdatePackageRequest>();
                CreateMap<CreatePackageRequest, Package>();

                CreateMap<Package, PackageResponse>();
                #endregion
                #region image
              

                CreateMap<Image, ImageResponse>();
                #endregion
                #region notification
                CreateMap<Notification, NotificationResponse>();
                #endregion

                #region shopweeklyreport
                CreateMap<ShopWeeklyReport, ShopWeeklyReportResponse>()
                    .ForMember(dest => dest.ShopName, opt => opt.MapFrom(src => src.Shop.Name))
                    .ForMember(dest => dest.ShopImage, opt => opt.MapFrom(src => src.Shop.Image))
                    .ForMember(dest => dest.Report, opt => opt.MapFrom(src => src.ReportData))
                    .ForMember(dest => dest.DateReport, opt => opt.MapFrom(src => src.CreateDate));
                #endregion
                #region openinghours
                CreateMap<OpeningHours, GetOpeningHoursForShopResonse>();
                CreateMap<OpeningHoursRequest, OpeningHours>();
                #endregion
            }
        }
    }
}
