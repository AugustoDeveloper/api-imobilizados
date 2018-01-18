using Imobilizados.Domain.Entity;
using MongoDB.Bson.Serialization;

namespace Imobilizados.Infrastructure.MongoDb
{
    public static class MongoDbMapping
    {
        public static void Map()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(Hardware)))
            {
                BsonClassMap.RegisterClassMap<Hardware>(mapper =>
                {
                    mapper.MapIdMember( f => f.Id)
                        .SetElementName("id");
                    mapper.MapMember(f => f.Name)
                        .SetElementName("name")
                        .SetIsRequired(true);
                    mapper.MapMember(f => f.Description)
                        .SetElementName("description")
                        .SetIsRequired(false)
                        .SetDefaultValue(string.Empty);
                    mapper.MapMember(f => f.Brand)
                        .SetElementName("brand")
                        .SetIsRequired(false)
                        .SetDefaultValue(string.Empty);
                    mapper.MapMember(f => f.FacoryCode)
                        .SetElementName("factory_code")
                        .SetIsRequired(false)
                        .SetDefaultValue(string.Empty);
                    mapper.MapMember(f => f.IsImmobilized)
                        .SetElementName("is_immobilized")
                        .SetIsRequired(true)
                        .SetDefaultValue(false);
                    mapper.MapMember(f => f.ImmobilizerFloor)
                        .SetElementName("Immobilizer_floor")
                        .SetIgnoreIfNull(true)
                        .SetIsRequired(false);                                    
                });
                        
            }

            if (BsonClassMap.IsClassMapRegistered(typeof(Floor)))
            {
                BsonClassMap.RegisterClassMap<Floor>(mapper =>
                {
                    mapper.MapIdMember( f => f.Id)
                        .SetElementName("id");
                    mapper.MapMember(f => f.Level)
                        .SetElementName("level")
                        .SetDefaultValue(-1);
                    mapper.MapMember(f => f.LevelName)
                        .SetElementName("level_name");    
                });
            }   
        }
    }
}