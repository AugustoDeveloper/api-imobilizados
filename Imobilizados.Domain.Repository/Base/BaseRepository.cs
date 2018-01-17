using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Imobilizados.Domain.Entity;

namespace Imobilizados.Domain.Repository.Base
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected BaseRepository()
        {
            Configure();
            Map();
        }

        public abstract TEntity Add(TEntity entity);
        public abstract Task<TEntity> AddAsync(TEntity entity);
        public abstract bool Delete(Expression<TEntity> criteria);
        public abstract Task<bool> DeleteAsync(Expression<TEntity> criteria);
        public abstract TEntity GetById(dynamic id);
        public abstract Task<TEntity> GetByIdAsync(dynamic id);
        public abstract List<TEntity> LoadAll();
        public abstract Task<List<TEntity>> LoadAllAsync();
        public abstract List<TEntity> LoadBy(Expression<TEntity> criteria);
        public abstract Task<List<TEntity>> LoadByAsync(Expression<TEntity> criteria);
        public abstract bool Update(TEntity entity);
        public abstract Task<bool> UpdateAsync(TEntity entity);
        protected abstract void Configure();
        protected abstract void Map();
        
    }
}