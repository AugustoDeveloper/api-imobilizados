using Moq;
using Xunit;
using FluentAssertions;
using Imobilizados.Domain.Repositories;
using Imobilizados.Application.Services.Interfaces;
using Imobilizados.Application.Services;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Threading;
using FloorEntity = Imobilizados.Domain.Entities.Floor;
using HardwareEntity = Imobilizados.Domain.Entities.Hardware;
using IHardwareEntity = Imobilizados.Domain.Entities.IHardware;
using ImmobilizedHardwareEntity = Imobilizados.Domain.Entities.ImmobilizedHardware;
using FloorDTO = Imobilizados.Application.DTOs.Floor;

namespace Imobilizados.Application.Tests.Services
{
    public class FloorServiceTest
    {
        [Trait(nameof(FloorService), "new()")]
        [Fact]
        public void Given_Try_Instantiation_When_Pass_Invalid_Args_Should_Thrown_ArgumentNullException()
        {
            Mock<IFloorRepository> mockFloorRepository = new();
            Mock<IHardwareRepository> mockHardwareRepository = new();
            Action createWithAllNullArgs = () => new FloorService(null, null);
            Action createWithFloorRepositoryNullArgs = () => new FloorService(null, mockHardwareRepository.Object);
            Action createWithHardwareRepositoryNullArgs = () => new FloorService(mockFloorRepository.Object, null);

            createWithAllNullArgs.Should().ThrowExactly<ArgumentNullException>().WithParameterName("floorRepository");
            createWithFloorRepositoryNullArgs.Should().ThrowExactly<ArgumentNullException>().WithParameterName("floorRepository");
            createWithHardwareRepositoryNullArgs.Should().ThrowExactly<ArgumentNullException>().WithParameterName("hardwareRepository");
        }

        [Trait(nameof(FloorService), nameof(IFloorService.GetAllAsync))]
        [Fact]
        public async Task Given_Call_GetAllAsync_When__Repo_Is_Empty_Should_Returns_Empty_Collection()
        {
            //arrange
            Mock<IFloorRepository> mockFloorRepository = new();
            Mock<IHardwareRepository> mockHardwareRepository = new();

            mockFloorRepository
                .Setup(f => f.GetAllAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<List<FloorEntity>>(null))
                .Verifiable();

            //act
            IFloorService service = new FloorService(mockFloorRepository.Object, mockHardwareRepository.Object);
            var floors = await service.GetAllAsync(default);

            //assert
            floors.Should().NotBeNull();
            floors.Should().BeEmpty();

            mockFloorRepository
                .Verify(f => f.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Trait(nameof(FloorService), nameof(IFloorService.GetAllAsync))]
        [Fact]
        public async Task Given_Call_GetAllAsync_When_Repo_Is_Not_Empty_Should_Returns_Not_Empty_Collection()
        {
            //arrange
            Mock<IFloorRepository> mockFloorRepository = new();
            Mock<IHardwareRepository> mockHardwareRepository = new();

            mockFloorRepository
                .Setup(f => f.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FloorEntity>
                {
                    new FloorEntity
                    {
                        Level = 1,
                        LevelName = "admin"
                    },
                    new FloorEntity
                    {
                        Level = 2,
                        LevelName = "rec"
                    }
                })
                .Verifiable();

            //act
            IFloorService service = new FloorService(mockFloorRepository.Object, mockHardwareRepository.Object);
            var floors = await service.GetAllAsync(default);

            //assert
            floors.Should().NotBeNull();
            floors.Should().HaveCount(2);
            floors.Should().Contain(f => f.Level == 1 && f.LevelName == "admin");
            floors.Should().Contain(f => f.Level == 2 && f.LevelName == "rec");

            mockFloorRepository
                .Verify(f => f.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Trait(nameof(FloorService), nameof(IFloorService.GetByIdAsync))]
        [Fact]
        public async Task Given_Call_GetByIdAsync_When_Repo_Is_Not_Empty_Should_Return_Instance_Value()
        {
            //arrange
            var id = Guid.NewGuid().ToString();
            Mock<IFloorRepository> mockFloorRepository = new();
            Mock<IHardwareRepository> mockHardwareRepository = new();

            mockFloorRepository
                .Setup(r => r.GetByIdAsync(It.Is<string>(s => s == id), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FloorEntity
                {
                    Id = id,
                    Level = 1,
                    LevelName = "Administracao"
                })
                .Verifiable();

            //act
            IFloorService service = new FloorService(mockFloorRepository.Object, mockHardwareRepository.Object);
            var floor = await service.GetByIdAsync(id, default);

            //assert
            mockFloorRepository
                .Verify(r => r.GetByIdAsync(It.Is<string>(s => s == id), It.IsAny<CancellationToken>()), Times.Once());
            floor.Should().NotBeNull();
            floor.Level.Should().Be(1);
            floor.LevelName.Should().Be("Administracao");
        }

        [Trait(nameof(FloorService), nameof(IFloorService.GetByIdAsync))]
        [Fact]
        public async Task Given_Call_GetByIdAsync_When_Repo_Is_Empty_Should_Return_Null_Value()
        {
            //arrange
            var id = Guid.NewGuid().ToString();
            Mock<IFloorRepository> mockFloorRepository = new();
            Mock<IHardwareRepository> mockHardwareRepository = new();

            mockFloorRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<FloorEntity>(null))
                .Verifiable();

            //act
            IFloorService service = new FloorService(mockFloorRepository.Object, mockHardwareRepository.Object);
            var floor = await service.GetByIdAsync(id, default);

            //assert
            mockFloorRepository
                .Verify(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
            floor.Should().BeNull();
        }

        [Trait(nameof(FloorService), nameof(IFloorService.InsertAsync))]
        [Fact]
        public async Task Given_Call_InsertAsync_When_Level_And_LevelName_Already_Inserted_Should_Returns_Inserted_Instance()
        {
            //arrange
            var id = Guid.NewGuid().ToString();
            var newFloor = new FloorDTO
            {
                Level = 1,
                LevelName = "admin"
            };

            Mock<IFloorRepository> mockFloorRepository = new();
            Mock<IHardwareRepository> mockHardwareRepository = new();

            mockFloorRepository
                .Setup(r => r.GetByLevelAsync(It.Is<int>(i => i == 1), It.Is<string>(s => s == "admin"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FloorEntity
                {
                    Id = id,
                    Level = 1,
                    LevelName = "admin"
                })
                .Verifiable();

            mockFloorRepository
                .Setup(f => f.InsertAsync(It.IsAny<FloorEntity>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            //act
            IFloorService service = new FloorService(mockFloorRepository.Object, mockHardwareRepository.Object);
            var floor = await service.InsertAsync(newFloor, default);

            //assert
            mockFloorRepository
                .Verify(r => r.GetByLevelAsync(It.Is<int>(i => i == 1), It.Is<string>(s => s == "admin"), It.IsAny<CancellationToken>()), Times.Once());
            mockFloorRepository
                .Verify(f => f.InsertAsync(It.IsAny<FloorEntity>(), It.IsAny<CancellationToken>()), Times.Never());

            floor.Should().NotBeNull();
            floor.Level.Should().Be(newFloor.Level);
            floor.LevelName.Should().Be(newFloor.LevelName);
        }

        [Trait(nameof(FloorService), nameof(IFloorService.InsertAsync))]
        [Fact]
        public async Task Given_Call_InsertAsync_When_Level_And_LevelName_Not_Exists_In_Repo_Should_Returns_New_One_Inserted()
        {
            //arrange
            var id = Guid.NewGuid().ToString();
            var newFloor = new FloorDTO
            {
                Level = 1,
                LevelName = "Administracao"
            };

            Mock<IFloorRepository> mockFloorRepository = new();
            Mock<IHardwareRepository> mockHardwareRepository = new();

            mockFloorRepository
                .Setup(r => r.GetByLevelAsync(It.Is<int>(i => i == 1), It.Is<string>(s => s == "Administracao"), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<FloorEntity>(null))
                .Verifiable();

            mockFloorRepository
                .Setup(f => f.InsertAsync(It.IsAny<FloorEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(id) 
                .Verifiable();

            mockFloorRepository
                .Setup(f => f.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FloorEntity
                {
                    Id = id,
                    Level = 1,
                    LevelName = "Administracao" 
                })
                .Verifiable();

            //act
            IFloorService service = new FloorService(mockFloorRepository.Object, mockHardwareRepository.Object);
            var floor = await service.InsertAsync(newFloor, default);

            //assert
            mockFloorRepository
                .Verify(r => r.GetByLevelAsync(It.Is<int>(i => i == 1), It.Is<string>(s => s == "Administracao"), It.IsAny<CancellationToken>()), Times.Once());

            mockFloorRepository
                .Verify(f => f.GetByIdAsync(It.Is<string>(s => s == id), It.IsAny<CancellationToken>()), Times.Once());

            mockFloorRepository
                .Verify(f => f.InsertAsync(It.IsAny<FloorEntity>(), It.IsAny<CancellationToken>()), Times.Once());

            floor.Should().NotBeNull();
            floor.Level.Should().Be(newFloor.Level);
            floor.LevelName.Should().Be(newFloor.LevelName);
        }

        [Trait(nameof(FloorService), nameof(IFloorService.DeleteAsync))]
        [Fact]
        public async Task Given_Call_DeleteAsync_When_Floor_Exists_But_Has_No_Hardware_Immobilized_Should_Do_Only_Deletion()
        {
            //arrange
            var id = Guid.NewGuid().ToString();

            Mock<IFloorRepository> mockFloorRepository = new();
            Mock<IHardwareRepository> mockHardwareRepository = new();

            mockHardwareRepository
                .Setup(h => h.UpdateAsync(It.IsAny<HardwareEntity>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            mockHardwareRepository
                .Setup(h => h.GetAllImmobilizedAtFloorAsync(It.Is<string>(s => s == id), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ImmobilizedHardwareEntity>())
                .Verifiable();

            mockFloorRepository
                .Setup(f => f.DeleteAsync(It.Is<string>(s => s == id), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            //act
            IFloorService service = new FloorService(mockFloorRepository.Object, mockHardwareRepository.Object);
            await service.DeleteAsync(id, default);

            //assert
            mockHardwareRepository
                .Verify(h => h.UpdateAsync(It.IsAny<HardwareEntity>(), It.IsAny<CancellationToken>()), Times.Never());

            mockHardwareRepository
                .Verify(h => h.GetAllImmobilizedAtFloorAsync(It.Is<string>(s => s == id), It.IsAny<CancellationToken>()), Times.Once());

            mockFloorRepository
                .Verify(f => f.DeleteAsync(It.Is<string>(s => s == id), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Trait(nameof(FloorService), nameof(IFloorService.DeleteAsync))]
        [Fact]
        public async Task Given_Call_DeleteAsync_When_Floor_Exists_In_Repo_And_Has_Immobilized_Hardware_Should_Do_Deletion_And_Update_Immobilized_Hardwares()
        {
            //arrange
            var id = Guid.NewGuid().ToString();

            Mock<IFloorRepository> mockFloorRepository = new();
            Mock<IHardwareRepository> mockHardwareRepository = new();

            mockHardwareRepository
                .Setup(h => h.UpdateAsync(It.IsAny<HardwareEntity>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            mockHardwareRepository
                .Setup(h => h.GetAllImmobilizedAtFloorAsync(It.Is<string>(s => s == id), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ImmobilizedHardwareEntity>
                        {
                            new ImmobilizedHardwareEntity(new HardwareEntity
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "Teclado",
                            }, new FloorEntity
                            {
                                Level = 1,
                                LevelName = "admin"
                            }),
                            new ImmobilizedHardwareEntity(new HardwareEntity
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "Mouse",
                            }, new FloorEntity
                            {
                                Level = 1,
                                LevelName = "admin"
                            }),
                            new ImmobilizedHardwareEntity(new HardwareEntity
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "Monitor",
                            }, new FloorEntity
                            {
                                Level = 1,
                                LevelName = "admin"
                            })
                        })
                .Verifiable();

            mockFloorRepository
                .Setup(f => f.DeleteAsync(It.Is<string>(s => s == id), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            //act
            IFloorService service = new FloorService(mockFloorRepository.Object, mockHardwareRepository.Object);
            await service.DeleteAsync(id, default);

            //assert
            mockHardwareRepository
                .Verify(h => h.UpdateAsync(It.IsAny<IHardwareEntity>(), It.IsAny<CancellationToken>()), Times.Exactly(3));

            mockHardwareRepository
                .Verify(h => h.GetAllImmobilizedAtFloorAsync(It.Is<string>(s => s == id), It.IsAny<CancellationToken>()), Times.Once());

            mockFloorRepository .Verify(f => f.DeleteAsync(It.Is<string>(s => s == id), It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
