using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using LiteDB;
using Imobilizados.Domain.Repositories;

namespace Imobilizados.Infrastructure.Repository.LiteDB.Base
{
    public abstract class LiteDatabaseRepositoryBase<TEntity> : IRepository<TEntity>
    {
        private readonly string collectionName;
        private readonly ILiteDatabase database;

        protected static BsonMapper Mapper => BsonMapper.Global;

        protected LiteDatabaseRepositoryBase(ILiteDatabase database, string collectionName = default)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
            this.collectionName = collectionName ?? $"{typeof(TEntity)}s";
        }

        protected void RunNewTask<T>(Func<T> func, TaskCompletionSource<T> tcs)
        {
            Task.Run(() =>
            {
                try
                {
                    var result = func.Invoke();
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
        }

        protected void RunNewTask(Action action, TaskCompletionSource tcs)
        {
            Task.Run(() =>
            {
                try
                {
                    action.Invoke();
                    tcs.SetResult();
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
        }

        protected TTo ToEntity<TTo>(BsonDocument document)
        {
            var typeName = document["_type"].AsString;
            var type = Type.GetType(typeName);

            var objectDoc = Mapper.Deserialize(type, document);
            var entity = (TTo)objectDoc;
            return entity;
        }

        protected ILiteCollection<BsonDocument> GetCollection(string collectionName)
        {
            return this.database.GetCollection(collectionName);
        }

        protected ILiteCollection<BsonDocument> GetCollection()
        {
            return this.database.GetCollection(this.collectionName);
        }

        protected ILiteCollection<TNewEntity> GetCollection<TNewEntity>()
        {
            var collectionName = $"{typeof(TNewEntity).Name}s";
            return this.database.GetCollection<TNewEntity>();
        }

        protected Task BuildTaskCompletionSource(Action source, CancellationToken cancellationToken)
        {
            var completion = new TaskCompletionSource();
            cancellationToken.Register(() => completion.TrySetCanceled());
            RunNewTask(() => source.Invoke(), completion);
            return completion.Task;
        }

        protected Task<T> BuildTaskCompletionSource<T>(Func<T> source, CancellationToken cancellationToken)
        {
            var completion = new TaskCompletionSource<T>();
            cancellationToken.Register(() => completion.TrySetCanceled());
            RunNewTask(() => source.Invoke(), completion);
            return completion.Task;
        }

        Task<List<TEntity>> IRepository<TEntity>.GetAllAsync(CancellationToken cancellationToken = default)
        {
            return BuildTaskCompletionSource<List<TEntity>>(() => GetAll(), cancellationToken);

            List<TEntity> GetAll()
            {
                var collection = GetCollection();
                var documents = collection.FindAll();
                var entities = documents?.Select(ToEntity<TEntity>).ToList();

                return entities;
            }
        }

        Task<TEntity> IRepository<TEntity>.GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return BuildTaskCompletionSource<TEntity>(() => GetById(id), cancellationToken);

            TEntity GetById(string hardwareId)
            {
                var collection = GetCollection();
                var document = collection.FindById(hardwareId);

                TEntity entity = default;
                if (document != null)
                {
                    entity = ToEntity<TEntity>(document);
                }

                return entity;
            }
        }

        Task<string> IRepository<TEntity>.InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return BuildTaskCompletionSource<string>(() => Insert(entity), cancellationToken);

            string Insert(TEntity newEntity)
            {
                var collection = GetCollection();
                var document = Mapper.ToDocument(newEntity);
                document.TryAdd("_type", newEntity.GetType().AssemblyQualifiedName);

                var id = Guid.NewGuid().ToString();
                collection.Insert(id, document);

                return id;
            }
        }

        Task IRepository<TEntity>.DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            return BuildTaskCompletionSource(() => Delete(id), cancellationToken);

            void Delete(string deletingId)
            {
                var collection = GetCollection();
                collection.Delete(deletingId);
            } 
        }

        Task<bool> IRepository<TEntity>.UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return BuildTaskCompletionSource<bool>(() => Update(entity), cancellationToken);

            bool Update(TEntity updateEntity)
            {
                var collection = GetCollection();
                var document = Mapper.ToDocument(updateEntity);
                
                var updated = collection.Update(document);

                return updated;
            }
        }
    }
}
