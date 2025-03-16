using Core;
using Core.Entities;
using Core.Repositories;
using Repository.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EContext _dbContext;
        private Hashtable _repository;
        public UnitOfWork(EContext dbContext)
        {
            _repository = new Hashtable();
            this._dbContext = dbContext;
        }
        public async Task<int> CompleteAsync()
        {
           return await _dbContext.SaveChangesAsync();
        }

        public ValueTask DisposeAsync()
            => _dbContext.DisposeAsync();

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            if (!_repository.ContainsKey(type))
            {
                var Repository = new GenericRepository<TEntity>(_dbContext);
                _repository.Add(type, Repository);
            }
            return _repository[type] as IGenericRepository<TEntity>;
        }
    }
}
