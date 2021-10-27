using Moq;
using LiteDB;
using FluentAssertions;
using Imobilizados.Domain.Entities;
using Imobilizados.Domain.Repositories;
using Imobilizados.Infrastructure.Repository.LiteDB;
using System.Linq;
using System.Collections.Generic;
using System;
using Xunit;
using System.Threading;
using System.Threading.Tasks;

namespace Imobilizados.Infrastructure.Repository.LiteDB.Tests
{
    public class HardwareRepositoryTest
    {
        [Trait(nameof(HardwareRepository), "new()")]
        [Fact]
        public void Given_Calling_Constructor_When_Passing_Invalid_Args_Should_Thrown_ArgumentNullException()
        {
            Action createWithAllNullArgs = () => new HardwareRepository(null);
            createWithAllNullArgs.Should().ThrowExactly<ArgumentNullException>();
        }

        [Trait(nameof(HardwareRepository), nameof(IHardwareRepository.GetAllAsync))]
        [Fact]
        public async Task Given_Calling_GetAllAsync_When_There_Is_In_Repo_Only_Immobilized_Should_Returns_Immobilized_List()
        {
            //arrange
            var id = Guid.NewGuid().ToString();
            var floorId = Guid.NewGuid().ToString();
            var hardwares = new[]
            {
                new BsonDocument
                {
                   ["_type"] = typeof(ImmobilizedHardware).AssemblyQualifiedName,
                   ["_id"] = id,
                   ["name"] = "mouse",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo mouse",
                   ["brand"] = "Apple",
                   ["floor"] = new BsonDocument
                   {
                       ["_id"] = floorId,
                       ["level"] = 1,
                       ["level_name"] = "Recepcao"
                   }
                },
                new BsonDocument
                {
                   ["_type"] = typeof(ImmobilizedHardware).AssemblyQualifiedName,
                   ["_id"] = id,
                   ["name"] = "teclado",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo teclado",
                   ["brand"] = "DELL",
                   ["floor"] = new BsonDocument
                   {
                       ["_id"] = floorId,
                       ["level"] = 1,
                       ["level_name"] = "Recepcao"
                   }
                }
            };

            Mock<ILiteCollection<BsonDocument>> mockCollection = new();
            Mock<ILiteDatabase> mockDatabase = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.FindAll())
                .Returns(hardwares)
                .Verifiable();

            IHardwareRepository repo = new HardwareRepository(mockDatabase.Object);
            var allHardwares = await repo.GetAllAsync();

            allHardwares.Should().AllBeOfType<ImmobilizedHardware>();
            allHardwares.Should().HaveCount(2);

            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()), Times.Once());
            mockCollection
                .Verify(c => c.FindAll(), Times.Once());
        }

        [Trait(nameof(HardwareRepository), nameof(IHardwareRepository.GetAllAsync))]
        [Fact]
        public async Task Given_Calling_GetAllAsync_When_There_Is_In_Repo_Only_Hardware_Should_Returns_Hardware_List()
        {
            //arrange
            var id = Guid.NewGuid().ToString();
            var floorId = Guid.NewGuid().ToString();
            var hardwares = new[]
            {
                new BsonDocument
                {
                   ["_type"] = typeof(Hardware).AssemblyQualifiedName,
                   ["_id"] = id,
                   ["name"] = "mouse",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo mouse",
                   ["brand"] = "Apple",
                },
                new BsonDocument
                {
                   ["_type"] = typeof(Hardware).AssemblyQualifiedName,
                   ["_id"] = id,
                   ["name"] = "teclado",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo teclado",
                   ["brand"] = "DELL",
                }
            };

            Mock<ILiteCollection<BsonDocument>> mockCollection = new();
            Mock<ILiteDatabase> mockDatabase = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.FindAll())
                .Returns(hardwares)
                .Verifiable();

            IHardwareRepository repo = new HardwareRepository(mockDatabase.Object);
            var allHardwares = await repo.GetAllAsync();

            allHardwares.Should().AllBeOfType<Hardware>();
            allHardwares.Should().HaveCount(2);

            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()), Times.Once());
            mockCollection
                .Verify(c => c.FindAll(), Times.Once());
        }

        [Trait(nameof(HardwareRepository), nameof(IHardwareRepository.GetAllAsync))]
        [Fact]
        public async Task Given_Calling_GetAllAsync_When_There_Is_In_Repo_Hardwares_And_Immobilizeds_Should_Returns_All_Items()
        {
            //arrange
            var id = Guid.NewGuid().ToString();
            var floorId = Guid.NewGuid().ToString();
            var hardwares = new[]
            {
                new BsonDocument
                {
                   ["_type"] = typeof(ImmobilizedHardware).AssemblyQualifiedName,
                   ["_id"] = id,
                   ["name"] = "mouse",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo mouse",
                   ["brand"] = "Apple",
                   ["floor"] = new BsonDocument
                   {
                       ["_id"] = floorId,
                       ["level"] = 1,
                       ["level_name"] = "Recepcao"
                   }
                },
                new BsonDocument
                {
                   ["_type"] = typeof(ImmobilizedHardware).AssemblyQualifiedName,
                   ["_id"] = id,
                   ["name"] = "teclado",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo teclado",
                   ["brand"] = "DELL",
                   ["floor"] = new BsonDocument
                   {
                       ["_id"] = floorId,
                       ["level"] = 1,
                       ["level_name"] = "Recepcao"
                   }
                },
                new BsonDocument
                {
                   ["_type"] = typeof(Hardware).AssemblyQualifiedName,
                   ["_id"] = id,
                   ["name"] = "mouse",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo mouse",
                   ["brand"] = "Apple",
                },
                new BsonDocument
                {
                   ["_type"] = typeof(Hardware).AssemblyQualifiedName,
                   ["_id"] = id,
                   ["name"] = "teclado",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo teclado",
                   ["brand"] = "DELL",
                }
            };

            Mock<ILiteCollection<BsonDocument>> mockCollection = new();
            Mock<ILiteDatabase> mockDatabase = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.FindAll())
                .Returns(hardwares)
                .Verifiable();

            IHardwareRepository repo = new HardwareRepository(mockDatabase.Object);
            var allHardwares = await repo.GetAllAsync();

            allHardwares.Should().Contain(h => h is ImmobilizedHardware);
            allHardwares.Should().Contain(h => h is Hardware);
            allHardwares.Should().HaveCount(4);

            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()), Times.Once());
            mockCollection
                .Verify(c => c.FindAll(), Times.Once());
        }

        [Trait(nameof(HardwareRepository), nameof(IHardwareRepository.GetAllAsync))]
        [Fact]
        public async Task Given_Calling_GetAllAsync_When_Exception_Occurs_Should_Thrown_Exception()
        {
            //arrange
            var id = Guid.NewGuid().ToString();
            var floorId = Guid.NewGuid().ToString();
            var hardwares = new[]
            {
                new BsonDocument
                {
                   ["_type"] = typeof(ImmobilizedHardware).AssemblyQualifiedName,
                   ["_id"] = id,
                   ["name"] = "mouse",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo mouse",
                   ["brand"] = "Apple",
                   ["floor"] = new BsonDocument
                   {
                       ["_id"] = floorId,
                       ["level"] = 1,
                       ["level_name"] = "Recepcao"
                   }
                },
                new BsonDocument
                {
                   ["_type"] = typeof(ImmobilizedHardware).AssemblyQualifiedName,
                   ["_id"] = id,
                   ["name"] = "teclado",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo teclado",
                   ["brand"] = "DELL",
                   ["floor"] = new BsonDocument
                   {
                       ["_id"] = floorId,
                       ["level"] = 1,
                       ["level_name"] = "Recepcao"
                   }
                },
                new BsonDocument
                {
                   ["_type"] = typeof(Hardware).AssemblyQualifiedName,
                   ["_id"] = id,
                   ["name"] = "mouse",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo mouse",
                   ["brand"] = "Apple",
                },
                new BsonDocument
                {
                   ["_type"] = typeof(Hardware).AssemblyQualifiedName,
                   ["_id"] = id,
                   ["name"] = "teclado",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo teclado",
                   ["brand"] = "DELL",
                }
            };

            Mock<ILiteCollection<BsonDocument>> mockCollection = new();
            Mock<ILiteDatabase> mockDatabase = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.FindAll())
                .Throws(new Exception())
                .Verifiable();

            IHardwareRepository repo = new HardwareRepository(mockDatabase.Object);
            Func<Task> getAllFunc = () => repo.GetAllAsync();

            await getAllFunc.Should().ThrowExactlyAsync<Exception>();

            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()), Times.Once());
            mockCollection
                .Verify(c => c.FindAll(), Times.Once());
        }

        [Trait(nameof(HardwareRepository), nameof(IHardwareRepository.GetAllAsync))]
        [Fact]
        public async Task Given_Calling_GetAllAsync_When_Get_Info_From_Repo_Takes_Longer_Than_Timeout_Should_Thrown_TaskCanceledException()
        {
            //arrange
            var cancellation = new CancellationTokenSource(40);
            var id = Guid.NewGuid().ToString();
            var floorId = Guid.NewGuid().ToString();
            var hardwares = new[]
            {
                new BsonDocument
                {
                   ["_type"] = typeof(ImmobilizedHardware).AssemblyQualifiedName,
                   ["_id"] = id,
                   ["name"] = "mouse",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo mouse",
                   ["brand"] = "Apple",
                   ["floor"] = new BsonDocument
                   {
                       ["_id"] = floorId,
                       ["level"] = 1,
                       ["level_name"] = "Recepcao"
                   }
                },
                new BsonDocument
                {
                   ["_type"] = typeof(ImmobilizedHardware).AssemblyQualifiedName,
                   ["_id"] = id,
                   ["name"] = "teclado",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo teclado",
                   ["brand"] = "DELL",
                   ["floor"] = new BsonDocument
                   {
                       ["_id"] = floorId,
                       ["level"] = 1,
                       ["level_name"] = "Recepcao"
                   }
                },
                new BsonDocument
                {
                   ["_type"] = typeof(Hardware).AssemblyQualifiedName,
                   ["_id"] = id,
                   ["name"] = "mouse",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo mouse",
                   ["brand"] = "Apple",
                },
                new BsonDocument
                {
                   ["_type"] = typeof(Hardware).AssemblyQualifiedName,
                   ["_id"] = id,
                   ["name"] = "teclado",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo teclado",
                   ["brand"] = "DELL",
                }
            };

            Mock<ILiteCollection<BsonDocument>> mockCollection = new();
            Mock<ILiteDatabase> mockDatabase = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.FindAll())
                .Callback(() => { Thread.Sleep(1000); })
                .Returns(Array.Empty<BsonDocument>())
                .Verifiable();

            IHardwareRepository repo = new HardwareRepository(mockDatabase.Object);
            Func<Task> getAllFunc = () => repo.GetAllAsync(cancellation.Token);

            await getAllFunc.Should().ThrowExactlyAsync<TaskCanceledException>();

            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()), Times.Once());
            mockCollection
                .Verify(c => c.FindAll(), Times.Once());
        }

        [Trait(nameof(HardwareRepository), nameof(IHardwareRepository.GetByIdAsync))]
        [Fact]
        public async Task Given_Calling_GetByIdAsync_When_Not_Exists_Id_Should_Returns_Null()
        {
            var id = Guid.NewGuid().ToString();
            var floorId = Guid.NewGuid().ToString();

            Mock<ILiteCollection<BsonDocument>> mockCollection = new();
            Mock<ILiteDatabase> mockDatabase = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.FindById(It.Is<BsonValue>(s => s == id)))
                .Returns<BsonDocument>(default)
                .Verifiable();

            //act
            IHardwareRepository repo = new HardwareRepository(mockDatabase.Object);
            var hardware = await repo.GetByIdAsync(id);

            //assert
            hardware.Should().BeNull();
            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()), Times.Once());

            mockCollection
                .Verify(c => c.FindById(It.Is<BsonValue>(s => s == id)), Times.Once());
        }

        [Trait(nameof(HardwareRepository), nameof(IHardwareRepository.GetByIdAsync))]
        [Fact]
        public async Task Given_Calling_GetByIdAsync_When_Exists_Id_In_Repo_And_It_Is_Immobilized_Should_Returns_Immobilized()
        {
            var id = Guid.NewGuid().ToString();
            var floorId = Guid.NewGuid().ToString();

            Mock<ILiteCollection<BsonDocument>> mockCollection = new();
            Mock<ILiteDatabase> mockDatabase = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.FindById(It.Is<BsonValue>(s => s == id)))
                .Returns(new BsonDocument
                {
                    ["_type"] = typeof(ImmobilizedHardware).AssemblyQualifiedName,
                    ["_id"] = id,
                    ["name"] = "mouse",
                    ["factory_code"] = Guid.NewGuid().ToString(),
                    ["description"] = "Novo mouse",
                    ["brand"] = "Apple",
                    ["floor"] = new BsonDocument
                    {
                        ["_id"] = floorId,
                        ["level"] = 1,
                        ["level_name"] = "Recepcao"
                    }
                })
                .Verifiable();

            //act
            IHardwareRepository repo = new HardwareRepository(mockDatabase.Object);
            var hardware = await repo.GetByIdAsync(id);

            //assert
            hardware.Should().NotBeNull();
            hardware.Should().BeOfType<ImmobilizedHardware>();
            hardware.Id.Should().Be(id);

            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()), Times.Once());

            mockCollection
                .Verify(c => c.FindById(It.Is<BsonValue>(s => s == id)), Times.Once());
        }

        [Trait(nameof(HardwareRepository), nameof(IHardwareRepository.GetByIdAsync))]
        [Fact]
        public async Task Given_Calling_GetByIdAsync_When_Exists_Id_In_Repo_And_It_Is_Hardware_Should_Returns_Hardware()
        {
            var id = Guid.NewGuid().ToString();
            var floorId = Guid.NewGuid().ToString();

            Mock<ILiteCollection<BsonDocument>> mockCollection = new();
            Mock<ILiteDatabase> mockDatabase = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.FindById(It.Is<BsonValue>(s => s == id)))
                .Returns(new BsonDocument
                {
                    ["_type"] = typeof(Hardware).AssemblyQualifiedName,
                    ["_id"] = id,
                    ["name"] = "mouse",
                    ["factory_code"] = Guid.NewGuid().ToString(),
                    ["description"] = "Novo mouse",
                    ["brand"] = "Apple",
                })
                .Verifiable();

            //act
            IHardwareRepository repo = new HardwareRepository(mockDatabase.Object);
            var hardware = await repo.GetByIdAsync(id);

            //assert
            hardware.Should().NotBeNull();
            hardware.Should().BeOfType<Hardware>();
            hardware.Id.Should().Be(id);

            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()), Times.Once());

            mockCollection
                .Verify(c => c.FindById(It.Is<BsonValue>(s => s == id)), Times.Once());
        }

        [Trait(nameof(HardwareRepository), nameof(IHardwareRepository.GetByIdAsync))]
        [Fact]
        public async Task Given_Calling_GetByIdAsync_When_Try_Get_Informantion_But_Takes_Longer_Than_Timeout_Should_Thrown_TaskCanceledException()
        {
            var id = Guid.NewGuid().ToString();
            var floorId = Guid.NewGuid().ToString();
            var cancellation = new CancellationTokenSource(40);

            Mock<ILiteCollection<BsonDocument>> mockCollection = new();
            Mock<ILiteDatabase> mockDatabase = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.FindById(It.Is<BsonValue>(s => s == id)))
                .Callback(() => { Thread.Sleep(1000); })
                .Returns(new BsonDocument
                {
                    ["_type"] = typeof(Hardware).AssemblyQualifiedName,
                    ["_id"] = id,
                    ["name"] = "mouse",
                    ["factory_code"] = Guid.NewGuid().ToString(),
                    ["description"] = "Novo mouse",
                    ["brand"] = "Apple",
                })
                .Verifiable();

            //act
            IHardwareRepository repo = new HardwareRepository(mockDatabase.Object);
            Func<Task> getByIdFunc = () => repo.GetByIdAsync(id, cancellation.Token);

            //assert
            await getByIdFunc.Should().ThrowExactlyAsync<TaskCanceledException>();

            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()), Times.Once());

            mockCollection
                .Verify(c => c.FindById(It.Is<BsonValue>(s => s == id)), Times.Once());
        }

        [Trait(nameof(HardwareRepository), nameof(IHardwareRepository.GetAllImobilizedAsync))]
        [Fact]
        public async Task Given_Calling_GetAllImmobilizedAsync_When_Not_Exists_Immobilizeds_Should_Returns_Empty_List()
        {
            var typeName = typeof(ImmobilizedHardware).AssemblyQualifiedName;

            var hardwares = new[]
            {
                new BsonDocument
                {
                   ["_type"] = typeName,
                   ["_id"] = Guid.NewGuid().ToString(),
                   ["name"] = "mouse",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo mouse",
                   ["brand"] = "Apple",
                   ["floor"] = new BsonDocument
                   {
                       ["_id"] = Guid.NewGuid().ToString(),
                       ["level"] = 1,
                       ["level_name"] = "Recepcao"
                   }
                },
                new BsonDocument
                {
                   ["_type"] = typeName,
                   ["_id"] = Guid.NewGuid().ToString(),
                   ["name"] = "teclado",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo teclado",
                   ["brand"] = "DELL",
                   ["floor"] = new BsonDocument
                   {
                       ["_id"] = Guid.NewGuid().ToString(),
                       ["level"] = 1,
                       ["level_name"] = "Recepcao"
                   }
                }
            };

            Mock<ILiteCollection<BsonDocument>> mockCollection = new();
            Mock<ILiteDatabase> mockDatabase = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.Find(It.IsAny<BsonExpression>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Array.Empty<BsonDocument>())
                .Verifiable();

            IHardwareRepository repo = new HardwareRepository(mockDatabase.Object);
            var allHardwares = await repo.GetAllImobilizedAsync();

            allHardwares.Should().AllBeOfType<ImmobilizedHardware>();
            allHardwares.Should().BeEmpty();

            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()), Times.Once());
            mockCollection
                .Verify(c => c.Find(It.IsAny<BsonExpression>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [Trait(nameof(HardwareRepository), nameof(IHardwareRepository.GetAllImobilizedAsync))]
        [Fact]
        public async Task Given_Calling_GetAllImmobilizedAsync_When_Exists_Immobilizeds_In_Repo_Should_Returns_Immobilizeds()
        {
            var typeName = typeof(ImmobilizedHardware).AssemblyQualifiedName;

            var hardwares = new[]
            {
                new BsonDocument
                {
                   ["_type"] = typeName,
                   ["_id"] = Guid.NewGuid().ToString(),
                   ["name"] = "mouse",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo mouse",
                   ["brand"] = "Apple",
                   ["floor"] = new BsonDocument
                   {
                       ["_id"] = Guid.NewGuid().ToString(),
                       ["level"] = 1,
                       ["level_name"] = "Recepcao"
                   }
                },
                new BsonDocument
                {
                   ["_type"] = typeName,
                   ["_id"] = Guid.NewGuid().ToString(),
                   ["name"] = "teclado",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo teclado",
                   ["brand"] = "DELL",
                   ["floor"] = new BsonDocument
                   {
                       ["_id"] = Guid.NewGuid().ToString(),
                       ["level"] = 1,
                       ["level_name"] = "Recepcao"
                   }
                }
            };

            Mock<ILiteCollection<BsonDocument>> mockCollection = new();
            Mock<ILiteDatabase> mockDatabase = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.Find(It.IsAny<BsonExpression>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(hardwares)
                .Verifiable();

            IHardwareRepository repo = new HardwareRepository(mockDatabase.Object);
            var allHardwares = await repo.GetAllImobilizedAsync();

            allHardwares.Should().AllBeOfType<ImmobilizedHardware>();
            allHardwares.Should().HaveCount(2);

            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()), Times.Once());
            mockCollection
                .Verify(c => c.Find(It.IsAny<BsonExpression>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [Trait(nameof(HardwareRepository), nameof(IHardwareRepository.GetAllImobilizedAsync))]
        [Fact]
        public async Task Given_Calling_GetAllImmobilizedAsync_When_Try_Get_Information_But_Takes_Longer_Than_Timeout_Should_Thrown_TaskCanceledException()
        {
            var cancellation = new CancellationTokenSource(40);
            var typeName = typeof(ImmobilizedHardware).AssemblyQualifiedName;

            var hardwares = new[]
            {
                new BsonDocument
                {
                   ["_type"] = typeName,
                   ["_id"] = Guid.NewGuid().ToString(),
                   ["name"] = "mouse",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo mouse",
                   ["brand"] = "Apple",
                   ["floor"] = new BsonDocument
                   {
                       ["_id"] = Guid.NewGuid().ToString(),
                       ["level"] = 1,
                       ["level_name"] = "Recepcao"
                   }
                },
                new BsonDocument
                {
                   ["_type"] = typeName,
                   ["_id"] = Guid.NewGuid().ToString(),
                   ["name"] = "teclado",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo teclado",
                   ["brand"] = "DELL",
                   ["floor"] = new BsonDocument
                   {
                       ["_id"] = Guid.NewGuid().ToString(),
                       ["level"] = 1,
                       ["level_name"] = "Recepcao"
                   }
                }
            };

            Mock<ILiteCollection<BsonDocument>> mockCollection = new();
            Mock<ILiteDatabase> mockDatabase = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.Find(It.IsAny<BsonExpression>(), It.IsAny<int>(), It.IsAny<int>()))
                .Callback(() => { Thread.Sleep(1000); })
                .Returns(hardwares)
                .Verifiable();

            IHardwareRepository repo = new HardwareRepository(mockDatabase.Object);
            Func<Task> getAllImmobilizedFunc = () => repo.GetAllImobilizedAsync(cancellation.Token);

            await getAllImmobilizedFunc.Should().ThrowExactlyAsync<TaskCanceledException>();
            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()), Times.Once());
            mockCollection
                .Verify(c => c.Find(It.IsAny<BsonExpression>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [Trait(nameof(HardwareRepository), nameof(IHardwareRepository.GetAllImobilizedAsync))]
        [Fact]
        public async Task Given_Calling_GetAllImmobilizedAtFloorAsync_When_There_Is_No_Immobilized_At_Floor_Should_Return_Empty_List()
        {
            var floorId = Guid.NewGuid().ToString();
            var cancellation = new CancellationTokenSource(40);
            var typeName = typeof(ImmobilizedHardware).AssemblyQualifiedName;

            var hardwares = new[]
            {
                new BsonDocument
                {
                   ["_type"] = typeName,
                   ["_id"] = Guid.NewGuid().ToString(),
                   ["name"] = "mouse",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo mouse",
                   ["brand"] = "Apple",
                   ["floor"] = new BsonDocument
                   {
                       ["_id"] = Guid.NewGuid().ToString(),
                       ["level"] = 1,
                       ["level_name"] = "Recepcao"
                   }
                },
                new BsonDocument
                {
                   ["_type"] = typeName,
                   ["_id"] = Guid.NewGuid().ToString(),
                   ["name"] = "teclado",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo teclado",
                   ["brand"] = "DELL",
                   ["floor"] = new BsonDocument
                   {
                       ["_id"] = Guid.NewGuid().ToString(),
                       ["level"] = 1,
                       ["level_name"] = "Recepcao"
                   }
                }
            };

            Mock<ILiteCollection<BsonDocument>> mockCollection = new();
            Mock<ILiteDatabase> mockDatabase = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.Find(It.IsAny<BsonExpression>(), It.IsAny<int>(), It.IsAny<int>()))
                //.Callback(() => { Thread.Sleep(1000); })
                //.Returns(hardwares)
                .Returns(Array.Empty<BsonDocument>())
                .Verifiable();

            //act
            IHardwareRepository repo = new HardwareRepository(mockDatabase.Object);
            var immobilizeds = await repo.GetAllImmobilizedAtFloorAsync(floorId);

            //assert
            immobilizeds.Should().BeEmpty();

            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()), Times.Once());
            mockCollection
                .Verify(c => c.Find(It.IsAny<BsonExpression>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [Trait(nameof(HardwareRepository), nameof(IHardwareRepository.GetAllImobilizedAsync))]
        [Fact]
        public async Task Given_Calling_GetAllImmobilizedAtFloorAsync_When_There_Is_Immobilized_At_Floor_Should_Return_Immobilized_List()
        {
            var floorId = Guid.NewGuid().ToString();
            var cancellation = new CancellationTokenSource(40);
            var typeName = typeof(ImmobilizedHardware).AssemblyQualifiedName;

            var hardwares = new[]
            {
                new BsonDocument
                {
                   ["_type"] = typeName,
                   ["_id"] = Guid.NewGuid().ToString(),
                   ["name"] = "mouse",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo mouse",
                   ["brand"] = "Apple",
                   ["floor"] = new BsonDocument
                   {
                       ["_id"] = Guid.NewGuid().ToString(),
                       ["level"] = 1,
                       ["level_name"] = "Recepcao"
                   }
                },
                new BsonDocument
                {
                   ["_type"] = typeName,
                   ["_id"] = Guid.NewGuid().ToString(),
                   ["name"] = "teclado",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo teclado",
                   ["brand"] = "DELL",
                   ["floor"] = new BsonDocument
                   {
                       ["_id"] = Guid.NewGuid().ToString(),
                       ["level"] = 1,
                       ["level_name"] = "Recepcao"
                   }
                }
            };

            Mock<ILiteCollection<BsonDocument>> mockCollection = new();
            Mock<ILiteDatabase> mockDatabase = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.Find(It.IsAny<BsonExpression>(), It.IsAny<int>(), It.IsAny<int>()))
                //.Callback(() => { Thread.Sleep(1000); })
                .Returns(hardwares)
                //.Returns(Array.Empty<BsonDocument>())
                .Verifiable();

            //act
            IHardwareRepository repo = new HardwareRepository(mockDatabase.Object);
            var immobilizeds = await repo.GetAllImmobilizedAtFloorAsync(floorId);

            //assert
            immobilizeds.Should().NotBeEmpty();
            immobilizeds.Should().HaveCount(2);
            immobilizeds.Should().AllBeOfType<ImmobilizedHardware>();

            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()), Times.Once());
            mockCollection
                .Verify(c => c.Find(It.IsAny<BsonExpression>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [Trait(nameof(HardwareRepository), nameof(IHardwareRepository.GetAllImobilizedAsync))]
        [Fact]
        public async Task Given_Calling_GetAllImmobilizedAtFloorAsync_When_Try_Get_Information_But_Takes_Longer_Than_Timeout_Should_Thrown_TaskCanceledException()
        {
            var floorId = Guid.NewGuid().ToString();
            var cancellation = new CancellationTokenSource(540);
            var typeName = typeof(ImmobilizedHardware).AssemblyQualifiedName;

            var hardwares = new[]
            {
                new BsonDocument
                {
                   ["_type"] = typeName,
                   ["_id"] = Guid.NewGuid().ToString(),
                   ["name"] = "mouse",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo mouse",
                   ["brand"] = "Apple",
                   ["floor"] = new BsonDocument
                   {
                       ["_id"] = Guid.NewGuid().ToString(),
                       ["level"] = 1,
                       ["level_name"] = "Recepcao"
                   }
                },
                new BsonDocument
                {
                   ["_type"] = typeName,
                   ["_id"] = Guid.NewGuid().ToString(),
                   ["name"] = "teclado",
                   ["factory_code"] = Guid.NewGuid().ToString(),
                   ["description"] = "Novo teclado",
                   ["brand"] = "DELL",
                   ["floor"] = new BsonDocument
                   {
                       ["_id"] = Guid.NewGuid().ToString(),
                       ["level"] = 1,
                       ["level_name"] = "Recepcao"
                   }
                }
            };

            Mock<ILiteCollection<BsonDocument>> mockCollection = new();
            Mock<ILiteDatabase> mockDatabase = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.Find(It.IsAny<BsonExpression>(), It.IsAny<int>(), It.IsAny<int>()))
                .Callback(() => { Thread.Sleep(1000); })
                //.Returns(hardwares)
                .Returns(Array.Empty<BsonDocument>())
                .Verifiable();

            //act
            IHardwareRepository repo = new HardwareRepository(mockDatabase.Object);
            Func<Task> getAllImmobilizedAtFloorFunc = () => repo.GetAllImmobilizedAtFloorAsync(floorId, cancellation.Token);

            //assert
            await getAllImmobilizedAtFloorFunc.Should().ThrowExactlyAsync<TaskCanceledException>();

            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()), Times.Once());
            mockCollection
                .Verify(c => c.Find(It.IsAny<BsonExpression>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [Trait(nameof(HardwareRepository), nameof(IHardwareRepository.InsertAsync))]
        [Fact]
        public async Task Given_Calling_InsertAsync_When_Instance_Is_Hardware_Should_Returns_New_Id()
        {
            var id = Guid.NewGuid().ToString();
            var cancellation = new CancellationTokenSource(540);
            var typeName = typeof(ImmobilizedHardware).AssemblyQualifiedName;

            var hardware = new Hardware
            {
                Id = id,
                Name = "teclado",
                Description = "novo teclado",
                FactoryCode = "123asdzx123",
                Brand = "DELL"
            };

            Mock<ILiteCollection<BsonDocument>> mockCollection = new();
            Mock<ILiteDatabase> mockDatabase = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.Insert(It.IsAny<BsonValue>(), It.IsAny<BsonDocument>()))
                .Verifiable();

            //act
            IHardwareRepository repo = new HardwareRepository(mockDatabase.Object);
            var newId = await repo.InsertAsync(hardware);

            //assert
            newId.Should().NotBe(id);

            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()), Times.Once());
            mockCollection
                .Verify(c => c.Insert(It.IsAny<BsonValue>(), It.IsAny<BsonDocument>()), Times.Once());
        }

        [Trait(nameof(HardwareRepository), nameof(IHardwareRepository.InsertAsync))]
        [Fact]
        public async Task Given_Calling_InsertAsync_When_Try_Insert_But_Takes_Longer_Than_Timeout_Should_Thrown_TaskCanceledException()
        {
            var id = Guid.NewGuid().ToString();
            var cancellation = new CancellationTokenSource(540);
            var typeName = typeof(ImmobilizedHardware).AssemblyQualifiedName;

            var hardware = new Hardware
            {
                Id = id,
                Name = "teclado",
                Description = "novo teclado",
                FactoryCode = "123asdzx123",
                Brand = "DELL"
            };

            Mock<ILiteCollection<BsonDocument>> mockCollection = new();
            Mock<ILiteDatabase> mockDatabase = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.Insert(It.IsAny<BsonValue>(), It.IsAny<BsonDocument>()))
                .Callback(() => { Thread.Sleep(1000); });

            //act
            IHardwareRepository repo = new HardwareRepository(mockDatabase.Object);
            Func<Task> insertFunc = () => repo.InsertAsync(hardware, cancellation.Token);

            //assert
            await insertFunc.Should().ThrowExactlyAsync<TaskCanceledException>();

            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()), Times.Once());

            mockCollection
                .Verify(c => c.Insert(It.IsAny<BsonValue>(), It.IsAny<BsonDocument>()), Times.Once());
        }

        [Trait(nameof(HardwareRepository), nameof(IHardwareRepository.UpdateAsync))]
        [Fact]
        public async Task Given_Calling_UpdateAsync_When_Instance_Is_Hardware_Should_Returns_Updated_Hardware()
        {
            var id = Guid.NewGuid().ToString();
            var hardwareTypeName = typeof(Hardware).AssemblyQualifiedName;

            var hardware = new Hardware
            {
                Id = id,
                Name = "teclado",
                Description = "novo teclado",
                FactoryCode = "123asdzx123",
                Brand = "DELL"
            };

            Mock<ILiteCollection<BsonDocument>> mockCollection = new();
            Mock<ILiteDatabase> mockDatabase = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.Update(It.IsAny<BsonDocument>()))
                .Returns(true)
                .Verifiable();

            //act
            IHardwareRepository repo = new HardwareRepository(mockDatabase.Object);
            var updated = await repo.UpdateAsync(hardware);

            //assert
            updated.Should().BeTrue();

            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()), Times.Once());

            mockCollection
                .Verify(c => c.Update(It.IsAny<BsonDocument>()), Times.Once());
        }

        [Trait(nameof(HardwareRepository), nameof(IHardwareRepository.UpdateAsync))]
        [Fact]
        public async Task Given_Calling_UpdateAsync_When_Try_Update_But_Takes_Longer_Than_Timeout_Should_Thrown_TaskCanceledException()
        {
            var cancellation = new CancellationTokenSource(400);
            var id = Guid.NewGuid().ToString();
            var hardwareTypeName = typeof(Hardware).AssemblyQualifiedName;

            var hardware = new Hardware
            {
                Id = id,
                Name = "teclado",
                Description = "novo teclado",
                FactoryCode = "123asdzx123",
                Brand = "DELL"
            };

            Mock<ILiteCollection<BsonDocument>> mockCollection = new();
            Mock<ILiteDatabase> mockDatabase = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.Update(It.IsAny<BsonDocument>()))
                .Callback(() => { Thread.Sleep(1000); })
                .Returns(true)
                .Verifiable();

            //act
            IHardwareRepository repo = new HardwareRepository(mockDatabase.Object);
            Func<Task> updateFunc = () => repo.UpdateAsync(hardware, cancellation.Token);

            //assert
            await updateFunc.Should().ThrowExactlyAsync<TaskCanceledException>();

            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()), Times.Once());
            mockCollection
                .Verify(c => c.Update(It.IsAny<BsonDocument>()), Times.Once());
        }

        [Trait(nameof(HardwareRepository), nameof(IHardwareRepository.UpdateAsync))]
        [Fact]
        public async Task Given_Calling_DeleteAsync_When_Pass_Valid_Id_Should_Do_Nothing()
        {
            var id = Guid.NewGuid().ToString();
            var hardwareTypeName = typeof(Hardware).AssemblyQualifiedName;

            Mock<ILiteCollection<BsonDocument>> mockCollection = new();
            Mock<ILiteDatabase> mockDatabase = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.Delete(It.IsAny<BsonValue>()))
                .Verifiable();

            //act
            IHardwareRepository repo = new HardwareRepository(mockDatabase.Object);
            await repo.DeleteAsync(id);

            //assert
            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()), Times.Once());

            mockCollection
                .Verify(c => c.Delete(It.IsAny<BsonValue>()), Times.Once());
        }

        [Trait(nameof(HardwareRepository), nameof(IHardwareRepository.UpdateAsync))]
        [Fact]
        public async Task Given_Calling_DeleteAsync_When_Try_Delete_But_Takes_Longer_Than_Timeout_Should_Thrown_TaskCanceledException()
        {
            var id = Guid.NewGuid().ToString();
            var cancellation = new CancellationTokenSource(400);
            var hardwareTypeName = typeof(Hardware).AssemblyQualifiedName;

            Mock<ILiteCollection<BsonDocument>> mockCollection = new();
            Mock<ILiteDatabase> mockDatabase = new();

            mockDatabase
                .Setup(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()))
                .Returns(mockCollection.Object)
                .Verifiable();

            mockCollection
                .Setup(c => c.Delete(It.IsAny<BsonValue>()))
                .Callback(() => { Thread.Sleep(1000); })
                .Returns(true)
                .Verifiable();

            //act
            IHardwareRepository repo = new HardwareRepository(mockDatabase.Object);
            Func<Task> deleteFunc = () => repo.DeleteAsync(id, cancellation.Token);

            //assert
            await deleteFunc.Should().ThrowExactlyAsync<TaskCanceledException>();
            mockDatabase
                .Verify(d => d.GetCollection(It.Is<string>(s => s == "hardwares"), It.IsAny<BsonAutoId>()), Times.Once());

            mockCollection
                .Verify(c => c.Delete(It.IsAny<BsonValue>()), Times.Once());
        }
    }
}
