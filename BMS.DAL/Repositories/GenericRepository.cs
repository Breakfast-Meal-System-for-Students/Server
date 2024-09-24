using BMS.Core.Domains.Entities.BaseEntities;
using BMS.Core.Helpers;
using BMS.Core.Models;
using BMS.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BMS.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {
        public GenericRepository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual DbSet<T> Entities => DbContext.Set<T>();

        public DbContext DbContext { get; }

        public void Add(T entity)
        {
            DbContext.Add(entity);
        }


        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await DbContext.AddAsync(entity, cancellationToken);
        }


        public void AddRange(IEnumerable<T> entities)
        {
            DbContext.AddRange(entities);
        }


        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await DbContext.AddRangeAsync(entities, cancellationToken);
        }


        public T Get(Expression<Func<T, bool>> expression)
        {
            return Entities.FirstOrDefault(expression);
        }



        public PaginatedResult GetPaginatedResult(
            int pageSize,
            int pageIndex,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Entities;
            return PaginationHelper.BuildPaginatedResultFullOptions(query, pageSize, pageIndex, filter, orderBy, includeProperties);
        }

        public IEnumerable<T> GetAll()
        {
            return Entities.AsEnumerable();
        }


        public IEnumerable<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return Entities.Where(expression).AsEnumerable();
        }


        public async Task<IList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await Entities.ToListAsync(cancellationToken);
        }


        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression,
            CancellationToken cancellationToken = default)
        {
            return await Entities.Where(expression).ToListAsync(cancellationToken);
        }


        public async Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await Entities.FirstOrDefaultAsync(expression, cancellationToken);
        }

        public async Task DeleteAsync(Guid id, bool saveChanges = true)
        {
            var entity = await Entities.FindAsync(id);
            await DeleteAsync(entity);

            if (saveChanges) await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity, bool saveChanges = true)
        {
            Entities.Remove(entity);
            if (saveChanges) await DbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities, bool saveChanges = true)
        {
            var enumerable = entities as T[] ?? entities.ToArray();
            if (enumerable.Any()) Entities.RemoveRange(enumerable);

            if (saveChanges) await DbContext.SaveChangesAsync();
        }

        public T Find(params object[] keyValues)
        {
            return Entities.Find(keyValues);
        }

        public virtual async Task<T?> FindAsync(params object[] keyValues)
        {
            return await Entities.FindAsync(keyValues);
        }
        public virtual async Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await Entities.FirstOrDefaultAsync(predicate);
        }
        public virtual IEnumerable<T> GetDetail(
     Expression<Func<T, bool>> filter = null,
    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
    string includeProperties = "",
    int? pageIndex = null, // Optional parameter for pagination (page number)
    int? pageSize = null)  // Optional parameter for pagination (number of records per page)
        {
            IQueryable<T> query = Entities;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // Implementing pagination
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                // Ensure the pageIndex and pageSize are valid
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10; // Assuming a default pageSize of 10 if an invalid value is passed

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            return query.ToList();
        }

        public async Task<IEnumerable<T>> GetDetailAsync(
     Expression<Func<T, bool>> filter = null,
    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
    string includeProperties = "",
    int? pageIndex = null, // Optional parameter for pagination (page number)
    int? pageSize = null)  // Optional parameter for pagination (number of records per page)
        {
            IQueryable<T> query = Entities;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // Implementing pagination
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                // Ensure the pageIndex and pageSize are valid
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10; // Assuming a default pageSize of 10 if an invalid value is passed

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            return await query.ToListAsync();
        }

        public T Get(Expression<Func<T, bool>> expression, object value)
        {
            throw new NotImplementedException();
        }
        public async Task<IQueryable<T>> FindAsyncAsQueryable(Expression<Func<T, bool>> predicate)
        {
            return await Task.FromResult(Entities.Where(predicate).AsQueryable());
        }
        public async Task<IQueryable<T>> GetAllAsyncAsQueryable()
        {
            return await Task.FromResult(Entities.AsQueryable());
        }
        // Adding a SoftDelete method
        public async Task SoftDeleteAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default)
        {
            if (entity is ISoftDelete softDeleteEntity)
            {
                softDeleteEntity.IsDeleted = true;
                softDeleteEntity.DeletedDate = DateTime.Now;

                DbContext.Update(entity);

                if (saveChanges)
                {
                    await DbContext.SaveChangesAsync(cancellationToken);
                }
            }
            else
            {
                throw new InvalidOperationException($"{nameof(T)} does not implement ISoftDelete");
            }
        }

        public async Task SoftDeleteByIdAsync(Guid id, bool saveChanges = true, CancellationToken cancellationToken = default)
        {
            var entity = await Entities.FindAsync(id);
            if (entity != null)
            {
                await SoftDeleteAsync(entity, saveChanges, cancellationToken);
            }
        }
    }
}
