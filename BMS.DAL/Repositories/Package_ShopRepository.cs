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


    public class Package_ShopRepository : GenericRepository<Core.Domains.Entities.Package_Shop>, IPackage_ShopRepository
    {
        public Package_ShopRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
