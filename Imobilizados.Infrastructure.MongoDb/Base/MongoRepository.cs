using System;
using Imobilizados.Domain.Entity;
using Imobilizados.Domain.Repository.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Imobilizados.Infrastructure.MongoDb.Base
{
    public abstract class MongoRepository<TEntity> : BaseRepository<TEntity> where TEntity : class, IEntity
    {
        private const string ConnectionStringSettingName = "";
        private const string DatabaseSettingName = "";
        protected abstract string CollectionName { get; }

        protected MongoRepository() : base() { }


        protected abstract void Map(BsonClassMap<TEntity> mapper);

        protected override void Map()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(TEntity)))
            {
                BsonClassMap.RegisterClassMap<TEntity>(this.Map);
            }   
        }
        
        protected override void Configure()
        {

        }
    }
}