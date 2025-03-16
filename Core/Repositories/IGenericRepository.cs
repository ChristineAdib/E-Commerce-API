using Core.Entities;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
        Task<T> GetEntityWithSpecAsync(ISpecifications<T> spec);

        Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec);
        Task AddAsync(T Entity);
        void Update(T Entity);
        void Delete(T Entity);
    }
}
