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
        void Update(string id, TEntity entity);
        Task UpdateAsync(string id, TEntity entity);
        void Delete(string id);
        Task DeleteAsync(string id);
        List<TEntity> LoadAll();
        Task<List<TEntity>> LoadAllAsync();
        TEntity GetById(string id);
        Task<TEntity> GetByIdAsync(string id);

        #endregion
    }
}