using System;

namespace Imobilizados.Repository.MongoDb
{
    public abstract class MongoRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private const string ConnectionStringSettingName = "";
        private const string DatabaseSettingName = "";
    }
}