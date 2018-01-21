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
        Task<TDto> AddAsync(TDto dto);
        Task UpdateAsync(string id, TDto dto);
        Task DeleteAsync(string id);
        Task<List<TDto>> LoadAllAsync();
        Task<TDto> GetByIdAsync(string id);
    }
}