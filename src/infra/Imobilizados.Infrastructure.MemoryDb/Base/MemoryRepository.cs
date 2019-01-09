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
        private static readonly Lazy<Dictionary<string, TEntity>> lazyDatabase = new Lazy<Dictionary<string, TEntity>>(() => new Dictionary<string, TEntity>());
        protected static Dictionary<string, TEntity> Database => lazyDatabase.Value;
        public MemoryRepository()
        {
        }

        public override void Add(TEntity entity)
        {
            string id = GenerateNewId();
            entity.Id = id;
            Database.Add(id, entity);
        }

        private string GenerateNewId()
        {
            var id = Guid.NewGuid();
            while (Database.ContainsKey(id.ToString()))
            {
                id = Guid.NewGuid();
            }

            return Convert.ToBase64String(id.ToByteArray());
        }

        public override Task AddAsync(TEntity entity)
        {
            return Task.Run(() => Add(entity));
        }

        public override void Delete(string id)
        {
            Database.Remove(id);
        }

        public override Task DeleteAsync(string id)
        {
            return Task.Run(() => Delete(id));
        }

        public override TEntity GetById(string id)
        {
            return Database.ContainsKey(id) ? Database[id] : default(TEntity);
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
            if (Database.ContainsKey(id))
            {
                Database[id] = entity;
            }
        }

        public override Task UpdateAsync(string id, TEntity entity)
        {
            return Task.Run(() => Update(id, entity));
        }
    }
}
