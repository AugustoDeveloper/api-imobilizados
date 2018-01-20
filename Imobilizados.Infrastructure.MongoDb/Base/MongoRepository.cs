using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Imobilizados.Domain.Entity;
using Imobilizados.Domain.Repository.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Imobilizados.Infrastructure.MongoDb.Base
{
    public abstract class MongoRepository<TEntity> : BaseRepository<TEntity> where TEntity : class, IEntity
    {
        private const string DatabaseSettingName = "ImmobilizedDb";
        protected abstract string CollectionName { get; }
        protected IMongoCollection<TEntity> Collection { get; }

        protected MongoRepository(IMongoClient mongoClient) : base()
        {
            var database = mongoClient.GetDatabase(DatabaseSettingName);
            Collection = database.GetCollection<TEntity>(CollectionName);
        }

        public override void Add(TEntity entity)
        {
            Collection.InsertOne(entity);
        }

        public override async Task AddAsync(TEntity entity)
        {
             await Collection.InsertOneAsync(entity);
        }

        public override void Delete(string id)
        {
             Collection.DeleteOne(e => e.Id == id);
        }

        public override async Task DeleteAsync(string id)
        {
            await Collection.DeleteOneAsync(e => e.Id == id);
        }

        public override TEntity GetById(string id)
        {
            var filter = Builders<TEntity>.Filter;
            var criteria = filter.Eq(e => e.Id, id);
            var entity = Collection.Find(criteria).SingleOrDefault();
            return entity;
        }

        public override async Task<TEntity> GetByIdAsync(string id)
        {
            var filter = Builders<TEntity>.Filter;
            var criteria = filter.Eq(e => e.Id, id);
            var entity = await Collection.Find(criteria).SingleOrDefaultAsync();
            return entity;
        }

        public override List<TEntity> LoadAll()
        {
            var filter = Builders<TEntity>.Filter;
            var criteria = filter.Empty;
            var batch = Collection.Find(criteria).ToList();
            return batch;
        }

        public override async Task<List<TEntity>> LoadAllAsync()
        {
            var filter = Builders<TEntity>.Filter;
            var criteria = filter.Empty;
            var batch = await Collection.Find(criteria).ToListAsync();
            return batch;
        }

        public override void Update(string id, TEntity entity)
        {
            Collection.ReplaceOne(e => e.Id == id, entity);
        }

        public override async Task UpdateAsync(string id, TEntity entity)
        {
            await Collection.ReplaceOneAsync(e => e.Id == id, entity);
        }
    }
}