using BMS.Core.Domains.Entities;
using BMS.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.DAL.Repositories
{
    public class CouponUsageRepository : GenericRepository<CouponUsage>, ICouponUsageRepository
    {
        public CouponUsageRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
