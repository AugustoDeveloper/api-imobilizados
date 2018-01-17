using Microsoft.Extensions.Configuration;
using SimpleInjector;
using MongoDB.Driver;

namespace Imobilizados.WebApi
{
    public static class MongoDbContainerExtension
    {
        public static void RegisterMongoDbRepository(this Container container, IConfiguration configuration, string connectionStringName = "Default")
        {
            container.RegisterSingleton<IMongoClient>(new MongoClient(configuration.GetConnectionString(connectionStringName)));
            
        }
    }
}