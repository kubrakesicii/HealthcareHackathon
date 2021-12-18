using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Results;

namespace DataAccess.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }

        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter);

        Task<DataResult<T>> InsertAsync(T entity);
        Task<Result> UpdateAsync(T entity);
        Task<Result> DeleteAsync(T entity);
    }
}
