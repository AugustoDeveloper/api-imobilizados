using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Imobilizados.Infrastructure.MongoDb;
using Imobilizados.Domain.Repository;
using Imobilizados.Application.Interfaces;
using Imobilizados.Application;
using MongoDB.Bson.Serialization;
using Imobilizados.Domain.Entity;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;

namespace Imobilizados.API.Utils
{
    public static class MongoDbConfiguration
    {
        public static void Map()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Hardware)))
            {
                BsonClassMap.RegisterClassMap<Hardware>(mapper =>
                {
                    mapper.AutoMap();
                    
                    mapper.MapIdMember( f => f.Id)     
                        .SetSerializer(new StringSerializer(BsonType.ObjectId))               
                        .SetIdGenerator(StringObjectIdGenerator.Instance);
                    mapper.MapMember(f => f.Name)                        
                        .SetIsRequired(true)
                        .SetElementName("name");
                    mapper.MapMember(f => f.Description)                        
                        .SetIsRequired(false)
                        .SetDefaultValue(string.Empty)
                        .SetElementName("description");
                    mapper.MapMember(f => f.Brand)
                        .SetIsRequired(false)
                        .SetDefaultValue(string.Empty)
                        .SetElementName("brand");
                    mapper.MapMember(f => f.FacoryCode)                        
                        .SetIsRequired(false)
                        .SetDefaultValue(string.Empty)
                        .SetElementName("factory_code");
                    mapper.MapMember(f => f.IsImmobilized)
                        .SetIsRequired(true)
                        .SetDefaultValue(false)
                        .SetElementName("is_immobilized");
                    mapper.MapMember(f => f.ImmobilizerFloor)
                        .SetIsRequired(false)
                        .SetElementName("immobilizer_floor");
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Floor)))
            {
                BsonClassMap.RegisterClassMap<Floor>(mapper =>
                {
                    mapper.UnmapMember(f => f.Id);
                    mapper.MapMember(f => f.Level)
                        .SetElementName("level")
                        .SetIsRequired(true)
                        .SetDefaultValue(-1);
                    mapper.MapMember(f => f.LevelName)
                        .SetElementName("level_name");    
                });
            }   
        
        }
    }
}