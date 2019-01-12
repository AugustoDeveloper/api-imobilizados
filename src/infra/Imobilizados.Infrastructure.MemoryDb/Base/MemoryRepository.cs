using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Imobilizados.Domain.Entity;
using Imobilizados.Domain.Repository.Base;

namespace Imobilizados.Infrastructure.MemoryDb.Base
{
    public class MemoryRepository<TEntity> : BaseRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly object lockObject = new object();
        private static readonly Lazy<Dictionary<int, TEntity>> lazyDatabase = new Lazy<Dictionary<int, TEntity>>(() => new Dictionary<int, TEntity>());
        protected static Dictionary<int, TEntity> Database => lazyDatabase.Value;
        public MemoryRepository()
        {
        }

        public override void Add(TEntity entity)
        {
            lock(lockObject)
            {
                int id = GenerateNewId();
                entity.Id = id.ToString().PadLeft(24, '0');
                Database.Add(id, entity);
            }
        }

        private int GenerateNewId() => (Database.Keys.Any() ?  Database.Keys.Max() + 1 : 1);

        public override Task AddAsync(TEntity entity)
        {
            return Task.Run(() => Add(entity));
        }

        public override void Delete(string id)
        {
            if (int.TryParse(id, out int key))
            {
                Database.Remove(key);
            }
        }

        public override Task DeleteAsync(string id)
        {
            return Task.Run(() => Delete(id));
        }

        public override TEntity GetById(string id)
        {
            return int.TryParse(id, out int key) && Database.ContainsKey(key) ? 
                Database[key] : 
                default(TEntity);
        }

        public override Task<TEntity> GetByIdAsync(string id)
        {
            return Task.Run(() => GetById(id));
        }

        public override List<TEntity> LoadAll()
        {
            return Database.Values.ToList();
        }

        public override Task<List<TEntity>> LoadAllAsync()
        {
            return Task.Factory.StartNew(LoadAll);
        }

        public override void Update(string id, TEntity entity)
        {
            if (int.TryParse(id, out int key) && Database.ContainsKey(key))
            {
                Database[key] = entity;
            }
        }

        public override Task UpdateAsync(string id, TEntity entity)
        {
            return Task.Run(() => Update(id, entity));
        }
    }
}
