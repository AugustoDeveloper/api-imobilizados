using System;
using Imobilizados.Application.Dtos;
using Imobilizados.Domain.Entity;
using System.Threading.Tasks;
using Imobilizados.Domain.Repository.Base;
using Imobilizados.Application.Interfaces;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;

namespace Imobilizados.Application
{
    public abstract class BaseService<TDto, TEntity, TRepository> : IService<TDto, TEntity, TRepository> 
                                            where TDto : class, IDto
                                             where TEntity : class, IEntity
                                             where TRepository : IRepository<TEntity>
    {
        
        protected TRepository Repository { get; }

        public  BaseService(TRepository repository)
        {
            Repository = repository;
        }

        public abstract Task<TEntity> AddAsync(TEntity entity);
        public abstract Task<bool> UpdateAsync(TEntity entity);
        public abstract Task<bool> DeleteAsync(Expression<TEntity> criteria);
        public abstract Task<List<TEntity>> LoadAllAsync();
        public abstract Task<List<TEntity>> LoadByAsync(Expression<TEntity> criteria);
        public abstract Task<TEntity> GetByIdAsync(dynamic id);

        protected TEntity Transform(TDto dto)
        {
            return Mapper.Map<TEntity>(dto);
        }

        protected TDto Transform(TEntity entity)
        {
            return Mapper.Map<TDto>(entity);
        }
    }
}
