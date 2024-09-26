using AutoMapper;
using BMS.BLL.Models;
using BMS.BLL.Services.BaseServices;
using BMS.BLL.Services.IServices;
using BMS.Core.Domains.Entities;
using BMS.Core.Domains.Enums;
using BMS.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services
{
    public class NotificationService : BaseService, INotificationService
    {
        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        public async Task<List<Notification>> GetAllNotificationsToSendMail(NotificationStatus status)
        {
            return (await _unitOfWork.NotificationRepository.GetAllAsyncAsQueryable())
                    .Include(a => a.User)
                    .Include(b => b.Shop)
                    .Include(c => c.Order)
                    .Where(x => x.Status == status.ToString()).ToList();
        }

        public async Task<ServiceActionResult> GetNotificationForShop(Guid ShopId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceActionResult> GetNotificationForUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task SaveChange()
        {
            await _unitOfWork.CommitAsync();
        }
    }
}
