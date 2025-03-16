using Core.Entities;
using Core.Repositories;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly EContext _dbContext;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(EContext dbContext )
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Category))
            {
                return (IReadOnlyList<T>)await _dbContext.Categories.Where(C => C.ParentId == null).Include(C => C.Children).ThenInclude(sc => sc.Products).ToListAsync();
            }
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
       
        private IQueryable<T> ApplySpecification(ISpecifications<T> spec)
        {
            return SpecificationEvalutor<T>.GetQuery(_dbContext.Set<T>(), spec);
        }
        
        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecification(Spec).CountAsync();
        }

        public async Task AddAsync(T entity)
            => await _dbContext.AddAsync(entity);

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public void Update(T Entity)
            => _dbContext.Set<T>().Update(Entity);

        public void Delete(T Entity)
            => _dbContext.Remove(Entity);

        public async Task<T> GetEntityWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }
    }
}
