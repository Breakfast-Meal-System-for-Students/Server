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
    public class FavouriteRepository : GenericRepository<Favourite>, IFavouriteRepository
    {
        public FavouriteRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
