using Microsoft.Extensions.Configuration;
using SimpleInjector;
using MongoDB.Driver;
using Imobilizados.Infrastructure.MongoDb;
using Imobilizados.Domain.Repository;
using Imobilizados.Application.Interfaces;
using Imobilizados.Application;

namespace Imobilizados.WebApi
{
    public static class MongoDbContainerExtension
    {
        public static void RegisterMongoDbRepositoriesAndMap(this Container container, IConfiguration configuration, string connectionStringName = "DefaultConnection")
        {
            container.RegisterSingleton<IMongoClient>(new MongoClient(configuration.GetConnectionString(connectionStringName)));
            container.Register<IHardwareRepository, HardwareRepository>();
            container.Register<IHardwareService, HardwareService>();
            MongoDbMapping.Map();      
        }
    }
}