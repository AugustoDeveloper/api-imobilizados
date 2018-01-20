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

        public async Task AddAsync(TDto dto)
        {
            var entity = TransformToEntity(dto);
            await Repository.AddAsync(entity);
        }
        
        public async Task UpdateAsync(string id, TDto dto)
        {
            var entity = TransformToEntity(dto);
            await Repository.UpdateAsync(id, entity);
        }

        public async Task DeleteAsync(string id)
        {
            await Repository.DeleteAsync(id);
        }

        public async Task<List<TDto>> LoadAllAsync()
        {
            var collection = await Repository.LoadAllAsync();
            return TransformToDto(collection);
        }

        public async Task<TDto> GetByIdAsync(string id)
        {
            var entity = await Repository.GetByIdAsync(id);
            var dto = TransformToDto(entity);
            return dto;
        }

        protected TEntity TransformToEntity(TDto dto)
        {
            return Mapper.Map<TEntity>(dto);
        }

        protected TDto TransformToDto(TEntity entity)
        {
            return Mapper.Map<TDto>(entity);
        }

        protected TNewEntity TransformToEntity<TNewEntity, TNewDto>(TNewDto dto) where TNewEntity : class, IEntity
                                                                              where TNewDto : class, IDto
        {
            return Mapper.Map<TNewEntity>(dto);
        }

        protected TNewDto TransformToDto<TNewEntity, TNewDto>(TNewEntity entity) where TNewEntity : class, IEntity
                                                                              where TNewDto : class, IDto
        {
            return Mapper.Map<TNewDto>(entity);
        }

        protected List<TDto> TransformToDto(List<TEntity> collection)
        {
            return Mapper.Map<List<TDto>>(collection);
        }
    }
}
