using System.Linq.Expressions;
using Moq;
using Xunit.Abstractions;
using FluentAssertions;
using System;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using LiteDB;
using Imobilizados.Domain.Entities;
using Imobilizados.Domain.Repositories;
using Imobilizados.Infrastructure.Repository.LiteDB;

namespace Imobilizados.Infrastructure.Repository.LiteDB.Tests
{
    public class StorageDocumentTest
    {
        public static BsonMapper Mapper => BsonMapper.Global;

        public StorageDocumentTest()
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

        [Fact]
        public void Given_()
        {
            File.Delete("test.db");
            using var database = new LiteDatabase("test.db");
            var genericCollection = database.GetCollection("hardwares");
            var floorId = Guid.NewGuid().ToString();

            var immobilized = new ImmobilizedHardware
            {
                Name = "Teclado",
                Brand = "Dell",
                Description = "Teclado Generico",
                FactoryCode = Guid.NewGuid().ToString(),
                Floor = new Floor
                {

                    Id = floorId,
                    Level = 1,
                    LevelName = "Recepcao"
                }
            };

            var typeName = typeof(ImmobilizedHardware).AssemblyQualifiedName;
            var immobilizedDoc = Mapper.ToDocument(immobilized);
            immobilizedDoc.Add("_type", typeName);
            var id = Guid.NewGuid().ToString();
            
            genericCollection.Insert(id, immobilizedDoc);

            //var documents = genericCollection.Find($"select * from hardwares where _type = '{typeName}' ");
            var documents = genericCollection.Find($"_type = '{typeName}' AND $.floor._id = '{floorId}'");

            var doc = documents.First();
            //var doc = genericCollection.FindById(id);
            var fullTypeName = doc["_type"].AsString;
            //fullTypeName.Should().BeEmpty();
            var type = Type.GetType(doc["_type"]);
            type.Should().NotBeNull();
            //type.FullName.Should().BeEmpty();

            var objectDoc = Mapper.Deserialize(type, doc);
            var hardware = objectDoc as IHardware;

            hardware.Should().NotBeNull();
        }

    }
}
