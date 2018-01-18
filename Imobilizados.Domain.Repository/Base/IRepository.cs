using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Imobilizados.Domain.Entity;

namespace Imobilizados.Domain.Repository.Base
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        #region 'Methods'

        void Add(TEntity entity);
        Task AddAsync(TEntity entity);
        TEntity Update(object id, TEntity entity);
        Task<TEntity> UpdateAsync(object id, TEntity entity);
        bool Delete(object id);
        Task<bool> DeleteAsync(object id);
        List<TEntity> LoadAll();
        Task<List<TEntity>> LoadAllAsync();
        TEntity GetById(object id);
        Task<TEntity> GetByIdAsync(object id);

        #endregion
    }
}