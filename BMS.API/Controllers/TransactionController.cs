using BMS.API.Controllers.Base;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;

namespace BMS.API.Controllers
{
    public class TransactionController : BaseApiController
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
            _baseService = (BaseService)transactionService;
        }
    }
}
