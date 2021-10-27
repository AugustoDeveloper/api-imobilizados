using Moq;
using LiteDB;
using FluentAssertions;
using Imobilizados.Domain.Entities;
using Imobilizados.Domain.Repositories;
using Imobilizados.Infrastructure.Repository.LiteDB.Extensions;
using Imobilizados.Infrastructure.Repository.LiteDB;
using System.Linq;
using System.Collections.Generic;
using System;
using Xunit;
using System.Threading;
using System.Threading.Tasks;

namespace Imobilizados.Infrastructure.Repository.LiteDB.Tests
{
    public class FloorRepositoryTest
    {
        public FloorRepositoryTest()
        {
            ServiceCollectionExtension.MapEntities();
        }

        [Trait(nameof(FloorRepository), "new()")]
        [Fact]
        public void Given_Calling_Constructor_When_Passing_Invalid_Args_Should_Thrown_ArgumentNullException()
        {
            Action createWithAllNullArgs = () => new FloorRepository(null);
            createWithAllNullArgs.Should().ThrowExactly<ArgumentNullException>();
        }

        [Trait(nameof(FloorRepository), nameof(IFloorRepository.GetByLevelAsync))]
        [Fact]
        public async Task Given_Calling_GetByLevelAsync_When_There_Is_No_Floor_Should_Return_Null()
        {
            //arrange
            Mock<ILiteDatabase> mockDatabase = new();
            Mock<ILiteCollection<BsonDocument>> mockCollection = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "floors"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.FindOne(It.IsAny<BsonExpression>()))
                .Returns<BsonDocument>(null)
                .Verifiable();

            //act
            IFloorRepository repo = new FloorRepository(mockDatabase.Object);
            var floor = await repo.GetByLevelAsync(1, "administracao");
            
            //assert
            floor.Should().BeNull();

            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "floors"), It.IsAny<BsonAutoId>()), Times.Once());

            mockCollection
                .Verify(c => c.FindOne(It.IsAny<BsonExpression>()), Times.Once());
        }

        [Trait(nameof(FloorRepository), nameof(IFloorRepository.GetByLevelAsync))]
        [Fact]
        public async Task Given_Calling_GetByLevelAsync_When_There_Is_A_Floor_Should_Return_A_Floor()
        {
            //arrange
            var id = Guid.NewGuid().ToString();
            var typeName = typeof(Floor).AssemblyQualifiedName;
            Mock<ILiteDatabase> mockDatabase = new();
            Mock<ILiteCollection<BsonDocument>> mockCollection = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "floors"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.FindOne(It.IsAny<BsonExpression>()))
                .Returns(new BsonDocument
                {
                    ["_id"] = id,
                    ["level"] = 1,
                    ["level_name"] = "admin",
                    ["_type"] = typeName
                })
                .Verifiable();

            //act
            IFloorRepository repo = new FloorRepository(mockDatabase.Object);
            var floor = await repo.GetByLevelAsync(1, "admin");

            //assert
            floor.Should().NotBeNull();
            floor.Id.Should().Be(id);
            floor.Level.Should().Be(1);
            floor.LevelName.Should().Be("admin");

            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "floors"), It.IsAny<BsonAutoId>()), Times.Once());

            mockCollection
                .Verify(c => c.FindOne(It.IsAny<BsonExpression>()), Times.Once());
        }

        [Trait(nameof(FloorRepository), nameof(IFloorRepository.GetByLevelAsync))]
        [Fact]
        public async Task Given_Calling_GetByLevelAsync_When_Try_Get_Information_But_Takes_Longer_Than_Timeout_Should_Thrown_TaskCanceledException()
        {
            //arrange
            var id = Guid.NewGuid().ToString();
            var typeName = typeof(Floor).AssemblyQualifiedName;
            var cancellation = new CancellationTokenSource(400);
            Mock<ILiteDatabase> mockDatabase = new();
            Mock<ILiteCollection<BsonDocument>> mockCollection = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "floors"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.FindOne(It.IsAny<BsonExpression>()))
                .Callback(() => { Thread.Sleep(1000); })
                .Returns(new BsonDocument
                {
                    ["_id"] = id,
                    ["level"] = 1,
                    ["level_name"] = "admin",
                    ["_type"] = typeName
                })
                .Verifiable();

            //act
            IFloorRepository repo = new FloorRepository(mockDatabase.Object);
            Func<Task> getByLevelFunc = () => repo.GetByLevelAsync(1, "admin", cancellation.Token);

            //assert
            await getByLevelFunc.Should().ThrowExactlyAsync<TaskCanceledException>();

            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "floors"), It.IsAny<BsonAutoId>()), Times.Once());

            mockCollection
                .Verify(c => c.FindOne(It.IsAny<BsonExpression>()), Times.Once());
        }
    }
}
