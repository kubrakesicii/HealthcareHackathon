using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Results;
using DataAccess.Contexts;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        // Burada verilen TEntity türü ile bir DBSet oluşturulur -> İlgili tabloya erişim sağlanır
        // DBContext objesi oluşturulur -> DB'ye erişim sağlanır
        private DbSet<T> _entity;
        public HealthcareContext _context;

        public virtual DbSet<T> Entity => _entity ??= _context.Set<T>();

        public IQueryable<T> Table => Entity;

        public IQueryable<T> TableNoTracking => Entity.AsNoTracking();

        public async Task<Result> DeleteAsync(T entity)
        {
            if (entity == null) return new ErrorDataResult<T>("Data not found!");

            try
            {
                foreach (var local in _context.Set<T>().Local)
                    _context.Entry(local).State = EntityState.Detached;

                Entity.Remove(entity);
                await _context.SaveChangesAsync();
                return new SuccessResult();

            }
            catch (Exception e)
            {
                //throw new InternalServerException("INTERNAL SERVER ERROR");
                return new ErrorDataResult<T>($"{e.Message}\n{e.InnerException}");
            }
        }


        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            return filter == null ? await TableNoTracking.ToListAsync() : await TableNoTracking.Where(filter).ToListAsync();
        }


        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await TableNoTracking.FirstOrDefaultAsync(filter);

        }

        public async Task<DataResult<T>> InsertAsync(T entity)
        {
            if (entity == null) return new ErrorDataResult<T>("Data not found");

            try
            {
                await Entity.AddAsync(entity);
                await _context.SaveChangesAsync();
                return new SuccessDataResult<T>(entity);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                //throw new ValidationException("MISSING REQUIRED FIELD");
                return new ErrorDataResult<T>($"{e.Message}\n{e.InnerException}");
            }
        }


        public async Task<Result> UpdateAsync(T entity)
        {
            if (entity == null) return new ErrorDataResult<T>("Data not found!");

            try
            {
                foreach (var local in _context.Set<T>().Local)
                    _context.Entry(local).State = EntityState.Detached;

                _context.Entry(entity).State = EntityState.Detached;
                _context.Update(entity);
                await _context.SaveChangesAsync();
                _context.Entry(entity).State = EntityState.Detached;
                return new SuccessDataResult<T>(entity);

            }
            catch (Exception e)
            {
                //throw new InternalServerException("INTERNAL SERVER ERROR");
                return new ErrorDataResult<T>($"{e.Message}\n{e.InnerException}");
            }
        }
    }
}
