using AutoMapper;
using BMS.BLL.Models.Requests.StudentApplication;
using BMS.BLL.Models.Responses.StudentApplication;
using BMS.BLL.Models;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.BLL.Utilities;
using BMS.Core.Domains.Entities;
using BMS.Core.Helpers;
using BMS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using BMS.Core.Exceptions;
using BMS.BLL.Helpers;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore;

namespace BMS.BLL.Services
{
    public class StudentApplicationService : BaseService, IStudentApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        public StudentApplicationService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, IEmailService emailService) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<ServiceActionResult> GetAllStudentApplication(StudentApplicationRequest queryParameters)
        {

            IQueryable<StudentApplication> StudentApplicationQueryable = (await _unitOfWork.StudentApplicationRepository.GetAllAsyncAsQueryable()).Include(a => a.University).Where(a => a.IsDeleted == false);



            if (!string.IsNullOrEmpty(queryParameters.Search))
            {
                StudentApplicationQueryable = StudentApplicationQueryable.Where(m => m.StudentId.Contains(queryParameters.Search));
            }

            StudentApplicationQueryable = queryParameters.IsDesc ? StudentApplicationQueryable.OrderByDescending(a => a.CreateDate) : StudentApplicationQueryable.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<StudentApplication, StudentApplicationResponse>(_mapper, StudentApplicationQueryable, queryParameters.PageSize, queryParameters.PageIndex);



            return new ServiceActionResult() { Data = paginationResult };
        }
        public async void AddStudentApplication(CreateStudentApplicationRequest request)
        {

            var StudentApplicationEntity = _mapper.Map<StudentApplication>(request);

            await _unitOfWork.StudentApplicationRepository.AddAsync(StudentApplicationEntity);
            // do something add feedback
            await _unitOfWork.CommitAsync();

            
        }

        public async Task<ServiceActionResult> UpdateStudentApplication(Guid id, UpdateStudentApplicationRequest request)
        {
            

            var StudentApplication = await _unitOfWork.StudentApplicationRepository.FindAsync(id) ?? throw new ArgumentNullException("StudentApplication is not exist");


            StudentApplication.LastUpdateDate = DateTimeHelper.GetCurrentTime();
            StudentApplication.StatusStudent = request.StatusStudent;
    
            await _unitOfWork.CommitAsync();

            if(StudentApplication.StatusStudent == Core.Domains.Enums.StudentStatus.ACCEPTED)
            {
                var user = await _userManager.FindByNameAsync(StudentApplication.Email);
                if (user == null)
                    throw new UserNotFoundException($"Invalid user with email {StudentApplication.Email}");

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _emailService.SendEmailConfirmationMoblieAsync(user, token);
            }
            if (StudentApplication.StatusStudent == Core.Domains.Enums.StudentStatus.DENIED)
            {
                await _emailService.SendEmailAsync(StudentApplication.Email, "YOU HAVE NEW INFORMATION FROM BMS", EmailHelper.GetRejectedEmailBody(StudentApplication.Email, "BMS", request.Message), true);
            }
            return new ServiceActionResult(true) { Data = StudentApplication };
        }


        public async Task<ServiceActionResult> GetStudentApplication(Guid id)
        {
           
           var StudentApplicationQueryable = (await _unitOfWork.StudentApplicationRepository.GetAllAsyncAsQueryable()).Include(a => a.University).Where(a => a.IsDeleted == false && a.Id ==id).FirstOrDefault();
            if(StudentApplicationQueryable is null)
            {
                return new ServiceActionResult(false) { IsSuccess= false,Detail = "Not Found Student Application" };
            }

            return new ServiceActionResult(true) { Data = StudentApplicationQueryable };
        }
        public async Task<ServiceActionResult> DeleteStudentApplication(Guid id)
        {
            await _unitOfWork.StudentApplicationRepository.SoftDeleteByIdAsync(id);
            return new ServiceActionResult(true)
            {
                Detail = "Delete Successfully"
            };
        }

    }
}
