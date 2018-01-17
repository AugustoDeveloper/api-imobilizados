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

        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
        bool Update(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        bool Delete(Expression<TEntity> criteria);
        Task<bool> DeleteAsync(Expression<TEntity> criteria);
        List<TEntity> LoadAll();
        Task<List<TEntity>> LoadAllAsync();
        
        List<TEntity> LoadBy(Expression<TEntity> criteria);
        Task<List<TEntity>> LoadByAsync(Expression<TEntity> criteria);
        TEntity GetById(dynamic id);
        Task<TEntity> GetByIdAsync(dynamic id);

        #endregion
    }
}