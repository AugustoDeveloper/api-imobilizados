using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Imobilizados.Domain.Entity;

namespace Imobilizados.Domain.Repository.Base
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected BaseRepository(){ }

        public abstract void Add(TEntity entity);
        public abstract Task AddAsync(TEntity entity);
        public abstract bool Delete(string id);
        public abstract Task<bool> DeleteAsync(string id);
        public abstract TEntity GetById(string id);
        public abstract Task<TEntity> GetByIdAsync(string id);
        public abstract List<TEntity> LoadAll();
        public abstract Task<List<TEntity>> LoadAllAsync();
        public abstract TEntity Update(string id, TEntity entity);
        public abstract Task<TEntity> UpdateAsync(string id, TEntity entity);
        
    }
}