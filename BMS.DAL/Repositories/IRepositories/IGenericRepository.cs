using BMS.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BMS.DAL.Repositories.IRepositories
{
    public interface IGenericRepository<T> where T : class, new()
    {
        DbSet<T> Entities { get; }
        DbContext DbContext { get; }
        T Get(Expression<Func<T, bool>> expression, object value);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Expression<Func<T, bool>> expression);
        Task<IQueryable<T>> FindAsyncAsQueryable(Expression<Func<T, bool>> expression);
        Task<IQueryable<T>> GetAllAsyncAsQueryable();
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);

        Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
        Task<IList<T>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression,
            CancellationToken cancellationToken = default);

        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);



        /// <summary>
        ///     Fin one item of an entity synchronous method
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        T Find(params object[] keyValues);

        /// <summary>
        ///     Find one item of an entity by asynchronous method
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        Task<T> FindAsync(params object[] keyValues);

        /// <summary>
        ///     Remove one item from an entity by asynchronous method
        /// </summary>
        /// <param name="id"></param>
        /// <param name="saveChanges"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id, bool saveChanges = true);

        /// <summary>
        ///     Remove one item from an entity by asynchronous method
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saveChanges"></param>
        /// <returns></returns>
        Task DeleteAsync(T entity, bool saveChanges = true);

        /// <summary>
        ///     Remove multiple items from an entity by asynchronous method
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="saveChanges"></param>
        /// <returns></returns>
        Task DeleteRangeAsync(IEnumerable<T> entities, bool saveChanges = true);
        PaginatedResult GetPaginatedResult(
            int pageSize,
            int pageIndex,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includeProperties);

        IEnumerable<T> GetDetail(
       Expression<Func<T, bool>> filter = null,
       Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
       string includeProperties = "",
       int? pageIndex = null, // Optional parameter for pagination (page number)
       int? pageSize = null   // Optional parameter for pagination (number of records per page)
   );
    }
}
