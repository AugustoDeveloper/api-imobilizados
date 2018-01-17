using System;
using Imobilizados.Application.Dtos;
using Imobilizados.Domain.Entity;
using Imobilizados.Domain.Repository.Base;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Imobilizados.Application.Interfaces
{
    public interface IService<TDto, TEntity, TRepository> where TDto : class, IDto
                                             where TEntity : class, IEntity
                                             where TRepository : IRepository<TEntity>
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(Expression<TEntity> criteria);
        Task<List<TEntity>> LoadAllAsync();
        Task<List<TEntity>> LoadByAsync(Expression<TEntity> criteria);
        Task<TEntity> GetByIdAsync(dynamic id);
    }
}