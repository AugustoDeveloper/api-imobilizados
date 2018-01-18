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
    public class FloorRepository : MongoRepository<Floor>, IFloorRepository
    {
        protected FloorRepository(IMongoClient mongoClient) : base(mongoClient){ }

        protected override string CollectionName => "Floor";
    }
}