using AutoMapper;
using BMS.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Helpers
{
    public class PaginationHelper
    {
        public static PaginatedResult BuildPaginatedResult<T, TDto>(IMapper? mapper, IQueryable<T> source, int pageSize, int pageIndex)
        {
            var total = source.Count();
            if (total == 0)
            {
                return new PaginatedResult
                {
                    PageIndex = 1,
                    PageSize = pageSize,
                    Data = new List<TDto>(),
                    LastPage = 1,
                    IsLastPage = true,
                    Total = total
                };
            }

            pageSize = Math.Max(1, pageSize);
            var lastPage = (int)Math.Ceiling((decimal)total / pageSize);
            lastPage = Math.Max(1, lastPage);
            pageIndex = Math.Max(1, Math.Min(pageIndex, lastPage));
            var isLastPage = pageIndex == lastPage;

            var paginatedResult = new PaginatedResult
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                LastPage = lastPage,
                IsLastPage = isLastPage,
                Total = total
            };

            IQueryable<T> paginatedQuery;


            paginatedQuery = source.Skip((pageIndex - 1) * pageSize).Take(pageSize);


            var resultList = paginatedQuery.ToList();
            paginatedResult.Data = mapper != null ? mapper.Map<IList<TDto>>(resultList) : (IList<TDto>)(object)resultList;

            return paginatedResult;
        }

        public static PaginatedResult BuildPaginatedResultFullOptions<T>(
             IQueryable<T> source,
             int pageSize,
             int pageIndex,
             Expression<Func<T, bool>> filter = null,
             Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
             params Expression<Func<T, object>>[] includeProperties)
             where T : class
        {
            IQueryable<T> query = source;

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            if (orderBy != null)
                query = orderBy(query);

            var total = query.Count();
            if (total == 0)
                return new PaginatedResult
                {
                    PageIndex = 1,
                    PageSize = pageSize,
                    Data = new List<T>(),
                    LastPage = 1,
                    IsLastPage = true,
                    Total = total
                };

            pageSize = Math.Max(1, pageSize);
            var lastPage = (long)Math.Ceiling((decimal)total / pageSize);
            lastPage = Math.Max(1, lastPage);
            pageIndex = Math.Min(pageIndex, (int)lastPage);
            var isLastPage = pageIndex == lastPage;

            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var paginatedResult = new PaginatedResult
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                LastPage = lastPage,
                IsLastPage = isLastPage,
                Total = total,
                Data = query.ToList()
            };

            return paginatedResult;
        }


        public static PaginatedResult BuildPaginatedResult<T>(IQueryable<T> source, int pageSize, int pageIndex)
        {
            return BuildPaginatedResult<T, T>(null, source, pageSize, pageIndex);
        }
    }
}
