using Microsoft.Extensions.Configuration;
using SimpleInjector;
using MongoDB.Driver;
using Imobilizados.Infrastructure.MongoDb;

namespace Imobilizados.WebApi
{
    public static class MongoDbContainerExtension
    {
        public static void RegisterMongoDbRepositoriesAndMap(this Container container, IConfiguration configuration, string connectionStringName = "Default")
        {
            container.RegisterSingleton<IMongoClient>(new MongoClient(configuration.GetConnectionString(connectionStringName)));
            MongoDbMapping.Map();      
        }
    }
}