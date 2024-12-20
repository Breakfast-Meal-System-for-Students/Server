using AutoMapper;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services
{
    public class WalletService : BaseService, IWalletService
    {
        public WalletService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
    }
}
