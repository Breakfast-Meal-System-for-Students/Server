using AutoMapper;
using BMS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BMS.BLL.Services.BaseServices
{
    public class BaseService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task Commit()
        {
            await _unitOfWork.CommitAsync();
        }
        public async Task Rollback()
        {
             await _unitOfWork.RollbackAsync();
        }
    }
}
