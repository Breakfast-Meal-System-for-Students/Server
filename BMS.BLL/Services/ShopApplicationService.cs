using AutoMapper;
using BMS.BLL.Helpers;
using BMS.BLL.Models;
using BMS.BLL.Models.Requests.Shop;
using BMS.BLL.Models.Requests.User;
using BMS.BLL.Models.Responses.Shop;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.BLL.Utilities;
using BMS.Core.Domains.Constants;
using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using BMS.Core.Exceptions;
using BMS.Core.Helpers;
using BMS.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services
{

    public class ShopApplicationService : BaseService, IShopApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly RoleManager<Role> _roleManager;
        private readonly IOpeningHoursService _openingHoursService;
        private readonly IGoogleMapService _googleMapService;
        private readonly IFileStorageService _fileStorageService;
        public ShopApplicationService(IUnitOfWork unitOfWork,
            IMapper mapper, UserManager<User> userManager, IEmailService emailService, RoleManager<Role> roleManager, IOpeningHoursService openingHoursService, IGoogleMapService googleMapService, IFileStorageService fileStorageService) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _emailService = emailService;
            _roleManager = roleManager;
            _openingHoursService = openingHoursService;
            _googleMapService = googleMapService;
            _fileStorageService = fileStorageService;
        }
        public async Task<ServiceActionResult> CreateShopApplication(CreateShopApplicationRequest applicationRequest)
        {
            var user = await _userManager.FindByEmailAsync(applicationRequest.Email);

            if (user != null)
            {
                return new ServiceActionResult(false)
                {
                    IsSuccess = false,
                    Detail = $"The Email {applicationRequest.Email} is already used in BMS System"
                };
            }

            // Validate opening and closing hours
            if (applicationRequest.from_hour < 5 || applicationRequest.from_hour > 12 ||
                applicationRequest.to_hour < 5 || applicationRequest.to_hour > 12 ||
                applicationRequest.from_minute < 0 || applicationRequest.from_minute > 60 ||
                applicationRequest.to_minute < 0 || applicationRequest.to_minute > 60)
            {
                return new ServiceActionResult(false)
                {
                    IsSuccess = false,
                    Detail = "Invalid time, The shop's opening and closing hours must be between 5:00 AM and 12:00 PM"
                };
            }

            var fromTime = new TimeSpan(applicationRequest.from_hour, applicationRequest.from_minute, 0);
            var toTime = new TimeSpan(applicationRequest.to_hour, applicationRequest.to_minute, 0);

            if (fromTime >= toTime)
            {
                return new ServiceActionResult(false)
                {
                    IsSuccess = false,
                    Detail = "Invalid time range. 'Open time' must be earlier than 'Close time'."
                };
            }

            // Map request to Shop entity
            var shopApplication = _mapper.Map<Shop>(applicationRequest);

            // Analyze latitude and longitude based on address
            try
            {
                var location = await _googleMapService.GetCoordinatesFromAddress(shopApplication.Address);
                if (location != null)
                {
                    shopApplication.lat = location.Lat;
                    shopApplication.lng = location.Lng;
                }
            }
            catch (Exception)
            {
                // Log exception or handle error
            }

            // Handle image upload
            if (applicationRequest.Image != null)
            {
                var imageUrl = await _fileStorageService.UploadFileBlobAsync(applicationRequest.Image);
                shopApplication.Image = imageUrl;
            }

            shopApplication.Rate = 5;

            try
            {
                // Save shop to the database
                await _unitOfWork.ShopRepository.AddAsync(shopApplication);
                await _unitOfWork.CommitAsync();

                // Add associated universities
                foreach (var universityId in applicationRequest.UniversityIds)
                {
                    var shopUniversity = new ShopUniversity
                    {
                        Id = Guid.NewGuid(),
                        ShopId = shopApplication.Id,
                        UniversityId = universityId
                    };
                    await _unitOfWork.ShopUniversityRepository.AddAsync(shopUniversity);
                }

                // Commit all changes
                await _unitOfWork.CommitAsync();

                // Add default opening hours
                await _openingHoursService.AddDefaultForShop(
                    shopApplication.Id,
                    applicationRequest.to_hour,
                    applicationRequest.from_hour,
                    applicationRequest.to_minute,
                    applicationRequest.from_minute);

                return new ServiceActionResult { IsSuccess = true };
            }
            catch
            {
                throw new BusinessRuleException("Error occurred while creating the shop application.");
            }
        }

        public async Task<ServiceActionResult> GetAllApplications(ShopApplicationRequest queryParameters)
        {
            IQueryable<Shop> applicationQuery = (await _unitOfWork.ShopRepository.GetAllAsyncAsQueryable()).Include(a => a.User).Include(a => a.ShopUniversities).ThenInclude(a=>a.University);

            var canParsed = Enum.TryParse(queryParameters.Status, true, out ShopStatus status);
            if (canParsed)
            {
                applicationQuery = applicationQuery.Where(m => m.Status == status);
            }

            if (!string.IsNullOrEmpty(queryParameters.Search))
            {
                applicationQuery = applicationQuery.Where(m => m.Name.Contains(queryParameters.Search) || m.Description.Equals(queryParameters.Search));
            }

            applicationQuery = queryParameters.IsDesc ? applicationQuery.OrderByDescending(a => a.CreateDate) : applicationQuery.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<Shop, ShopApplicationResponse>(_mapper, applicationQuery, queryParameters.PageSize, queryParameters.PageIndex);

            return new ServiceActionResult(true) { Data = paginationResult };

        }

        public async Task<ServiceActionResult> GetApplication(Guid id)
        {
            WeekDay currentDay = DateTimeHelper.GetCurrentWeekDay()+1;
            // var application = await _unitOfWork.ShopRepository.FindAsync(id) ?? throw new ArgumentNullException("Application is not exist");
            var applicationQuery = (await _unitOfWork.ShopRepository.GetAllAsyncAsQueryable()).Include(a => a.User).Include(a => a.ShopUniversities).ThenInclude(a => a.University).Where(x => x.Id == id).FirstOrDefault();
            var openCloseShop =await _unitOfWork.OpeningHoursRepository.FindAsync(a => a.ShopId == id&&(a.day== currentDay));
            var openCloseShopTomorow = await _unitOfWork.OpeningHoursRepository.FindAsync(a => a.ShopId == id && (a.day == (WeekDay)(((int)currentDay + 1) % 7)));

            var returnApplication = _mapper.Map<ShopApplicationDetailResponse>(applicationQuery);
            if(returnApplication is not null)
            {
                returnApplication.From_Hour = openCloseShop.from_hour;
                returnApplication.To_Hour = openCloseShop.to_hour;
                returnApplication.From_Minune = openCloseShop.from_minute;
                returnApplication.To_Minune = openCloseShop.to_minute;
            }
            if (openCloseShopTomorow is not null)
            {
                returnApplication.From_HourTomorow = openCloseShopTomorow.from_hour;
                returnApplication.To_HourTomorow = openCloseShopTomorow.to_hour;
                returnApplication.From_MinuneTomorow = openCloseShopTomorow.from_minute;
                returnApplication.To_MinuneTomorow = openCloseShopTomorow.to_minute;
            }

            return new ServiceActionResult(true) { Data = returnApplication };
        }

        public async Task<ServiceActionResult> ReviewApplication(Guid id, string status, string message)
        {
            var application = await _unitOfWork.ShopRepository.FindAsync(id) ?? throw new ArgumentNullException("Application is not exist");

            if (status.Equals(application.Status.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return new ServiceActionResult(false) { IsSuccess = false, Detail = $"Application is already in {status} status" };
            }
            else if (application.Status.ToString().Equals(ShopStatus.ACCEPTED.ToString(), StringComparison.OrdinalIgnoreCase)
                || application.Status.ToString().Equals(ShopStatus.DENIED.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return new ServiceActionResult(false) {IsSuccess = false, Detail = $"Application is already in {application.Status.ToString()} status. Can not modify." };
            }
            else if (status.Equals(ShopStatus.ACCEPTED.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                var account = await RegisterShopAsync(application);
                application.Status = ShopStatus.ACCEPTED;
                var user = await _userManager.FindByEmailAsync(application.Email);
                application.UserId = user.Id;
                await _unitOfWork.CommitAsync();
                await _emailService.SendEmailAsync(application.Email, "YOU HAVE NEW INFORMATION FROM BMS", EmailHelper.GetAcceptedEmailBody("breakfastmealsystem@gmail.com", application.Email, account.Password), true);
            }
            else if (status.Equals(ShopStatus.DENIED.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                application.Status = ShopStatus.DENIED;
                await _unitOfWork.CommitAsync();
                await _emailService.SendEmailAsync(application.Email, "YOU HAVE NEW INFORMATION FROM BMS", EmailHelper.GetRejectedEmailBody(application.Name, "BMS", message), true);
            }
            else
            {
                return new ServiceActionResult(false) { Detail = $"The status must be In (ACCEPTED, DENIED)" };
            }

            return new ServiceActionResult(true)
            {
                Detail = "Successfully, Please The Email is sended to Shop"
            };
        }

        private async Task<LoginUser> RegisterShopAsync(Shop application)
        {
            var user = await _userManager.FindByEmailAsync(application.Email);

            if (user != null)
            {
                if (!await _roleManager.RoleExistsAsync(UserRoleConstants.SHOP))
                {
                    await _roleManager.CreateAsync(new Role { Name = UserRoleConstants.SHOP });
                }
                var roleResultIn = await _userManager.AddToRoleAsync(user, UserRoleConstants.SHOP);

                if (!roleResultIn.Succeeded)
                {
                    var error = roleResultIn.Errors.First().Description;
                    throw new AddRoleException(error);
                }
                List<OpeningHours> openingHours1 = OpeningHoursCreationHelper.GenerateOpeningHours(application.Id);
                await _unitOfWork.OpeningHoursRepository.AddRangeAsync(openingHours1.AsEnumerable());
                return new LoginUser() { Email = application.Email, Password = "Your currently password in my system" };
            }

            var userEntity = new User
            {
                Email = application.Email,
                UserName = application.Email,
                PhoneNumber = application.PhoneNumber,
                FirstName = application.Name,
                ShopId = application.Id,
                Phone = application.PhoneNumber,
            };

            //  public string? Password { get; set; } = null!;
            var password = AccountCreationHelper.GenerateRandomPassword();

            var result = await _userManager.CreateAsync(userEntity, password);

            if (!result.Succeeded)
            {
                var error = result.Errors.First().Description;
                throw new Exception(error);
            }

            if (!await _roleManager.RoleExistsAsync(UserRoleConstants.SHOP))
            {
                await _roleManager.CreateAsync(new Role { Name = UserRoleConstants.SHOP });
            }
            var roleResult = await _userManager.AddToRoleAsync(userEntity, UserRoleConstants.SHOP);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(userEntity);
                throw new AddRoleException("Can not create account with role SHOP");
            }

            userEntity.EmailConfirmed = true;
            await _userManager.UpdateAsync(userEntity);

            List<OpeningHours> openingHours2 = OpeningHoursCreationHelper.GenerateOpeningHours(application.Id);
            await _unitOfWork.OpeningHoursRepository.AddRangeAsync(openingHours2.AsEnumerable());
            await _unitOfWork.CommitAsync();
            await _unitOfWork.WalletRepository.AddAsync(
                new Wallet()
                {
                    UserId = userEntity.Id,
                    WalletName = $"BMS Wallet - {userEntity.Email}"
                }
                );
            await _unitOfWork.CommitAsync();
            return new LoginUser() { Email = application.Email, Password = password };
        }



    }
}
