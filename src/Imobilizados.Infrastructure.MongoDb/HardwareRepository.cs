using Imobilizados.Infrastructure.MongoDb.Base;
using Imobilizados.Domain.Entity;
using Imobilizados.Domain.Repository.Base;
using Imobilizados.Domain.Repository;
using MongoDB.Bson.Serialization;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Imobilizados.Infrastructure.MongoDb
{
    public class HardwareRepository : MongoRepository<Hardware>, IHardwareRepository
    {
        public HardwareRepository(IMongoClient mongoClient) : base(mongoClient){ }

        protected override string CollectionName => "Hardwares";

        public async Task<List<Hardware>> LoadByFloorAsync(Floor floor)
        {
            return await Collection.Find( h => h.IsImmobilized && h.ImmobilizerFloor.Level == floor.Level).ToListAsync();
        }

        public async Task<List<Hardware>> LoadByIsImmobilizedAsync(bool isImmobilized)
        {
            return await Collection.Find( h => h.IsImmobilized == isImmobilized).ToListAsync();
        }

    }
}