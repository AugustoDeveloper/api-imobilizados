using Imobilizados.Domain.Repositories;
using Imobilizados.Infrastructure.Repository.LiteDB;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Imobilizados.Domain.Entities;

namespace Imobilizados.Infrastructure.Repository.LiteDB.Extensions
{
    public static class ServiceCollectionExtension
    {
        private static BsonMapper Mapper => BsonMapper.Global;

        public static void MapEntities()
        {
            Mapper
                .Entity<Hardware>()
                .Id(h => h.Id, false)
                .Field(h => h.Brand, "brand")
                .Field(h => h.Description, "description")
                .Field(h => h.FactoryCode, "factory_code")
                .Field(h => h.Name, "name");

            Mapper
                .Entity<ImmobilizedHardware>()
                .Id(h => h.Id, false)
                .Field(h => h.Brand, "brand")
                .Field(h => h.Description, "description")
                .Field(h => h.FactoryCode, "factory_code")
                .Field(h => h.Name, "name")
                .Field(h => h.Floor, "floor");

            Mapper
                .Entity<Floor>()
                .Id(f => f.Id, false)
                .Field(f => f.Level, "level")
                .Field(f => f.LevelName, "level_name");
        }

        public static IServiceCollection AddLiteDB(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<ILiteDatabase>(_ => new LiteDatabase(connectionString));
            services.AddScoped<IHardwareRepository, HardwareRepository>();
            services.AddScoped<IFloorRepository, FloorRepository>();

            MapEntities();

            return services;
        }
    }
}
