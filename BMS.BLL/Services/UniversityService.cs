using AutoMapper;
using BMS.BLL.Models.Requests.University;
using BMS.BLL.Models.Responses.University;
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

namespace BMS.BLL.Services
{

    public class UniversityService : BaseService, IUniversityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UniversityService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceActionResult> GetAllUniversity(UniversityRequest queryParameters)
        {

            IQueryable<University> UniversityQueryable = (await _unitOfWork.UniversityRepository.GetAllAsyncAsQueryable()).Where(a => a.IsDeleted == false);



            if (!string.IsNullOrEmpty(queryParameters.Search))
            {
                UniversityQueryable = UniversityQueryable.Where(m => m.Name.Contains(queryParameters.Search));
            }

            UniversityQueryable = queryParameters.IsDesc ? UniversityQueryable.OrderByDescending(a => a.CreateDate) : UniversityQueryable.OrderBy(a => a.CreateDate);

            var paginationResult = PaginationHelper
            .BuildPaginatedResult<University, UniversityResponse>(_mapper, UniversityQueryable, queryParameters.PageSize, queryParameters.PageIndex);



            return new ServiceActionResult() { Data = paginationResult };
        }
        public async Task<ServiceActionResult> AddUniversity(CreateUniversityRequest request)
        {
            if (request.Name.Trim() == "")
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Name is not Empty"
                };
            }
            if (request.Name.Trim() == "")
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Name is not Empty"
                };
            }
;

            var UniversityEntity = _mapper.Map<University>(request);

            await _unitOfWork.UniversityRepository.AddAsync(UniversityEntity);
            // do something add feedback
            await _unitOfWork.CommitAsync();

            return new ServiceActionResult(true) { Data = UniversityEntity };
        }

        public async Task<ServiceActionResult> UpdateUniversity(Guid id, UpdateUniversityRequest request)
        {
            if (request.Name.Trim() == "")
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Name is not Empty"
                };
            }
            if (request.Name.Trim() == "")
            {
                return new ServiceActionResult(false)
                {
                    Detail = "Name is not Empty"
                };
            }

            var University = await _unitOfWork.UniversityRepository.FindAsync(id) ?? throw new ArgumentNullException("University is not exist");


            University.LastUpdateDate = DateTimeHelper.GetCurrentTime();
            University.Name = request.Name;
            University.Address = request.Address;
            University.EndMail = request.EndMail;
            University.Abbreviation = request.Abbreviation;
            await _unitOfWork.CommitAsync();

            return new ServiceActionResult(true) { Data = University };
        }


        public async Task<ServiceActionResult> GetUniversity(Guid id)
        {
            var University = await _unitOfWork.UniversityRepository
         .FindAsync(c => c.Id == id && c.IsDeleted == false)
         ?? throw new ArgumentNullException("University does not exist or has been deleted");
            var returnUniversity = _mapper.Map<UniversityResponse>(University);

            return new ServiceActionResult(true) { Data = returnUniversity };
        }
        public async Task<ServiceActionResult> DeleteUniversity(Guid id)
        {
            await _unitOfWork.UniversityRepository.SoftDeleteByIdAsync(id);
            return new ServiceActionResult(true)
            {
                Detail = "Delete Successfully"
            };
        }
       
    }
}
