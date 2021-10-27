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
using IHardwareEntity = Imobilizados.Domain.Entities.IHardware;
using ImmobilizedHardwareEntity = Imobilizados.Domain.Entities.ImmobilizedHardware;
using HardwareEntity = Imobilizados.Domain.Entities.Hardware;
using FloorEntity = Imobilizados.Domain.Entities.Floor;
using HardwareDTO = Imobilizados.Application.DTOs.Hardware;

namespace Imobilizados.Application.Tests.Services
{
    public class HardwareServiceTest
    {
        [Trait(nameof(HardwareService), "new()")]
        [Fact]
        public void Given_Try_Instantiation_When_Pass_Invalid_Args_Should_Thrown_ArgumentNullException()
        {
            Mock<IHardwareRepository> mockRepository= new();
            Mock<IFloorRepository> mockFloorRepository = new();

            Action createWithAllNullArgs = () => new HardwareService(null, null);
            Action createWithHardwareRepositoryNullArgs = () => new HardwareService(null, mockFloorRepository.Object);
            Action createWithFloorRepositoryNullArgs = () => new HardwareService(mockRepository.Object, null);

            createWithAllNullArgs.Should().ThrowExactly<ArgumentNullException>().WithParameterName("repository");
            createWithHardwareRepositoryNullArgs.Should().ThrowExactly<ArgumentNullException>().WithParameterName("repository");
            createWithFloorRepositoryNullArgs.Should().ThrowExactly<ArgumentNullException>().WithParameterName("floorRepository");
        }

        [Trait(nameof(HardwareService), nameof(IHardwareService.GetAllAsync))]
        [Fact]
        public async Task Given_Call_GetAllAsync_When_IsImobilized_Is_True_But_Repo_Is_Empty_Should_Returns_Empty_Collection()
        {
            Mock<IFloorRepository> mockFloorRepository = new();
            Mock<IHardwareRepository> mockRepository = new();

            mockRepository
                .Setup(r => r.GetAllImobilizedAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<List<IHardwareEntity>>(null));

            mockRepository
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<List<IHardwareEntity>>(null));

            IHardwareService service = new HardwareService(mockRepository.Object, mockFloorRepository.Object);
            var hardwares = await service.GetAllAsync(true, default);

            hardwares.Should().NotBeNull();
            hardwares.Should().BeEmpty();
        }

        [Trait(nameof(HardwareService), nameof(IHardwareService.GetAllAsync))]
        [Fact]
        public async Task Given_Call_GetAllAsync_When_IsImobilized_Is_False_But_Repo_Is_Empty_Should_Returns_Empty_Collection()
        {
            Mock<IFloorRepository> mockFloorRepository = new();
            Mock<IHardwareRepository> mockRepository = new();

            mockRepository
                .Setup(r => r.GetAllImobilizedAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<List<IHardwareEntity>>(null));

            mockRepository
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<List<IHardwareEntity>>(null));

            IHardwareService service = new HardwareService(mockRepository.Object, mockFloorRepository.Object);
            var hardwares = await service.GetAllAsync(false, default);

            hardwares.Should().NotBeNull();
            hardwares.Should().BeEmpty();
        }

        public async Task Given_Call_GetAllAsync_When_IsImobilized_Is_True_Should_Returns_Imobilized_Hardwares()
        {
            //arrange
            Mock<IFloorRepository> mockFloorRepository = new();
            Mock<IHardwareRepository> mockRepository = new();

            mockRepository
                .Setup(r => r.GetAllImobilizedAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<IHardwareEntity>
                {
                    new HardwareEntity
                    {
                        Brand = "Dell",
                        Description = "Mouse Dell",
                        FactoryCode = "123IMOB321",
                        Name = "Mouse"
                    },
                    new HardwareEntity
                    {
                        Brand = "Dell",
                        Description = "Teclado Dell",
                        FactoryCode = "321IMOB123",
                        Name = "Teclado"
                    },
                })
                .Verifiable();

            mockRepository
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<IHardwareEntity>
                {
                    new HardwareEntity
                    {
                        Brand = "Dell",
                        Description = "Mouse Dell",
                        FactoryCode = "21HYR09",
                        Name = "Mouse"
                    },
                    new HardwareEntity
                    {
                        Brand = "Dell",
                        Description = "Teclado Dell",
                        FactoryCode = "21HYX09",
                        Name = "Teclado"
                    },
                })
                .Verifiable();

            //act
            IHardwareService service = new HardwareService(mockRepository.Object, mockFloorRepository.Object);
            var hardwares = await service.GetAllAsync(true, default);

            //assert
            mockRepository
                .Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Never());

            mockRepository
                .Verify(r => r.GetAllImobilizedAsync(It.IsAny<CancellationToken>()), Times.Once());

            hardwares.Should().NotBeNull();
            hardwares.Should().HaveCount(2);
            hardwares.Should().Contain(h => h.FactoryCode == "123IMOB321");
            hardwares.Should().Contain(h => h.FactoryCode == "321IMOB123");
        }

        [Trait(nameof(HardwareService), nameof(IHardwareService.GetAllAsync))]
        [Fact]
        public async Task Given_Call_GetAllAsync_When_IsImobilized_Is_False_Should_Returns_Not_Imobilized_Hardwares()
        {
            //arrange
            Mock<IFloorRepository> mockFloorRepository = new();
            Mock<IHardwareRepository> mockRepository = new();

            mockRepository
                .Setup(r => r.GetAllImobilizedAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<IHardwareEntity>
                {
                    new HardwareEntity
                    {
                        Brand = "Dell",
                        Description = "Mouse Dell",
                        FactoryCode = "123IMOB321",
                        Name = "Mouse"
                    },
                    new HardwareEntity
                    {
                        Brand = "Dell",
                        Description = "Teclado Dell",
                        FactoryCode = "321IMOB123",
                        Name = "Teclado"
                    },
                })
                .Verifiable();


            mockRepository
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<IHardwareEntity>
                {
                    new HardwareEntity
                    {
                        Brand = "Dell",
                        Description = "Mouse Dell",
                        FactoryCode = "21HYR09",
                        Name = "Mouse"
                    },
                    new HardwareEntity
                    {
                        Brand = "Dell",
                        Description = "Teclado Dell",
                        FactoryCode = "21HYX09",
                        Name = "Teclado"
                    },
                })
                .Verifiable();

            //act
            IHardwareService service = new HardwareService(mockRepository.Object, mockFloorRepository.Object);
            var hardwares = await service.GetAllAsync(false, default);

            //assert
            mockRepository
                .Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once());

            mockRepository
                .Verify(r => r.GetAllImobilizedAsync(It.IsAny<CancellationToken>()), Times.Never());

            hardwares.Should().NotBeNull();
            hardwares.Should().HaveCount(2);
            hardwares.Should().Contain(h => h.FactoryCode == "21HYR09");
            hardwares.Should().Contain(h => h.FactoryCode == "21HYX09");
        }

        [Trait(nameof(HardwareService), nameof(IHardwareService.GetByIdAsync))]
        [Fact]
        public async Task Given_Call_GetByIdAsync_When_Repo_Is_Empty_Should_Return_Null_Value()
        {
            //arrange
            var id = Guid.NewGuid().ToString();
            Mock<IFloorRepository> mockFloorRepository = new();
            Mock<IHardwareRepository> mockRepository = new();

            mockRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IHardwareEntity>(null))
                .Verifiable();

            //act
            IHardwareService service = new HardwareService(mockRepository.Object, mockFloorRepository.Object);
            var hardware = await service.GetByIdAsync(id, default);

            //assert
            mockRepository
                .Verify(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
            hardware.Should().BeNull();
        }

        [Trait(nameof(HardwareService), nameof(IHardwareService.GetByIdAsync))]
        [Fact]
        public async Task Given_Call_GetByIdAsync_When_Id_Exists_In_Repo_Should_Return_Empty_List()
        {
            //arrange
            var id = Guid.NewGuid().ToString();
            Mock<IFloorRepository> mockFloorRepository = new();
            Mock<IHardwareRepository> mockRepository = new();

            mockRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HardwareEntity
                {
                    Id = id,
                    Brand = "Dell",
                    Description = "Mouse Dell",
                    FactoryCode = "123IMOB321",
                    Name = "Mouse"
                })
                .Verifiable();

            //act
            IHardwareService service = new HardwareService(mockRepository.Object, mockFloorRepository.Object);
            var hardware = await service.GetByIdAsync(id, default);

            //assert
            mockRepository
                .Verify(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());

            hardware.Should().NotBeNull();
            hardware.Id.Should().Be(id);
            hardware.Name.Should().Be("Mouse");
            hardware.FactoryCode.Should().Be("123IMOB321");
        }

        [Trait(nameof(HardwareService), nameof(IHardwareService.InsertAsync))]
        [Fact]
        public async Task Given_Call_InsertAsync_When_Not_Exists_In_Repo_Should_Returns_Hardware_With_New_Id()
        {
            //arrange
            var id = Guid.NewGuid().ToString();
            Mock<IFloorRepository> mockFloorRepository = new();
            Mock<IHardwareRepository> mockRepository = new();

            mockRepository
                .Setup(r => r.InsertAsync(It.Is<HardwareEntity>(e => e.Id == null && e.Name == "Mouse"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(id)
                .Verifiable();

            mockRepository
                .Setup(r => r.GetByIdAsync(It.Is<string>(s => s == id), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HardwareEntity
                {
                    Id = id,
                    Name = "Mouse",
                    FactoryCode = "123IMOB321"
                })
                .Verifiable();

            //act
            IHardwareService service = new HardwareService(mockRepository.Object, mockFloorRepository.Object);
            var hardware = await service.InsertAsync(new HardwareDTO
            {
                Id = null,
                Name = "Mouse",
                FactoryCode = "123IMOB321",
            }, default);

            //assert
            mockRepository
                .Verify(f => f.GetByIdAsync(It.Is<string>(s => s == id), It.IsAny<CancellationToken>()), Times.Once());

            mockRepository
                .Verify(r => r.InsertAsync(It.Is<HardwareEntity>(e => e.Id == null && e.Name == "Mouse"), It.IsAny<CancellationToken>()), Times.Once());

            hardware.Should().NotBeNull();
            hardware.Id.Should().Be(id);
            hardware.Name.Should().Be("Mouse");
            hardware.FactoryCode.Should().Be("123IMOB321");
        }

        [Trait(nameof(HardwareService), nameof(IHardwareService.DeleteAsync))]
        [Fact]
        public async Task Given_Call_DeleteAsync_When_Exists_In_Repo_Should_Deleted_Entity_From_Repository()
        {
            //arrange
            var id = Guid.NewGuid().ToString();
            var repo = new List<HardwareEntity>
            {
                new HardwareEntity
                {
                    Brand = "Dell",
                    Description = "Dell Teclado 1",
                    FactoryCode = "MXCV0923AZ",
                    Name = "Teclado",
                    Id = id
                }
            };

            Mock<IFloorRepository> mockFloorRepository = new();
            Mock<IHardwareRepository> mockRepository = new();

            mockRepository
                .Setup(r => r.DeleteAsync(It.Is<string>(s => s == id), It.IsAny<CancellationToken>()))
                .Returns((string id, CancellationToken cancellationToken) =>
                {
                    repo.RemoveAll(r => r.Id == id);
                    return Task.CompletedTask;
                })
                .Verifiable();

            //act 
            IHardwareService service = new HardwareService(mockRepository.Object, mockFloorRepository.Object);
            await service.DeleteAsync(id, default);

            //assert
            mockRepository
                .Verify(r => r.DeleteAsync(It.Is<string>(s => s == id), It.IsAny<CancellationToken>()), Times.Once());

            repo.Should().BeEmpty();
        }

        [Trait(nameof(HardwareService), nameof(IHardwareService.UpdateAsync))]
        [Fact]
        public async Task Given_Call_UpdateAsync_When_Id_Not_Exists_In_Repo_Should_Return_Null_Instance()
        {
            //arrange
            var id = Guid.NewGuid().ToString();
            Mock<IFloorRepository> mockFloorRepository = new();
            Mock<IHardwareRepository> mockRepository = new();

            mockRepository
                .Setup(r => r.GetByIdAsync(It.Is<string>(s => s == id), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IHardwareEntity>(null))
                .Verifiable();

            mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<HardwareEntity>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            IHardwareService service = new HardwareService(mockRepository.Object, mockFloorRepository.Object);
            var updated = await service.UpdateAsync(id, new HardwareDTO(), default);

            updated.Should().BeFalse();

            mockRepository
                .Verify(r => r.GetByIdAsync(It.Is<string>(s => s == id), It.IsAny<CancellationToken>()), Times.Once());

            mockRepository
                .Verify(r => r.UpdateAsync(It.IsAny<HardwareEntity>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Trait(nameof(HardwareService), nameof(IHardwareService.UpdateAsync))]
        [Fact]
        public async Task Given_Call_UpdateAsync_When_Id_Exists_In_Repo_And_Updated_Information_Should_Return_Updated_Hardware()
        {
            //arrange
            var id = Guid.NewGuid().ToString();
            Mock<IFloorRepository> mockFloorRepository = new();
            Mock<IHardwareRepository> mockRepository = new();

            mockRepository
                .Setup(r => r.GetByIdAsync(It.Is<string>(s => s == id), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HardwareEntity
                {
                    Id = id,
                    Name = "Teclado Normal",
                    Brand = "Apple",
                    FactoryCode = "001"
                })
                .Verifiable();

            mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<HardwareEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .Verifiable();

            IHardwareService service = new HardwareService(mockRepository.Object, mockFloorRepository.Object);
            var updated = await service.UpdateAsync(id, new HardwareDTO
            {
                Name = "Teclado Mecanico",
                Brand = "Dell",
                FactoryCode = "123MXCV008"
            }, default);

            updated.Should().BeTrue();

            mockRepository
                .Verify(r => r.GetByIdAsync(It.Is<string>(s => s == id), It.IsAny<CancellationToken>()), Times.Once());

            mockRepository
                .Verify(r => r.UpdateAsync(It.IsAny<HardwareEntity>(), It.IsAny<CancellationToken>()), Times.Once());

        }

        [Trait(nameof(HardwareService), nameof(IHardwareService.ImmobilizeAsync))]
        [Fact]
        public async Task Given_Call_ImmobilizedAsync_When_Hardware_Not_Exists_In_Repo_Should_Returns_Not_Updated()
        {
            //arrange
            var hardwareId = Guid.NewGuid().ToString();
            var floorId = Guid.NewGuid().ToString();
            Mock<IFloorRepository> mockFloorRepository = new();
            Mock<IHardwareRepository> mockRepository = new();

            mockFloorRepository
                .Setup(r => r.GetByIdAsync(It.Is<string>(s => s == hardwareId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FloorEntity
                {
                    Id = floorId
                })
                .Verifiable();

            mockRepository
                .Setup(r => r.GetByIdAsync(It.Is<string>(s => s == hardwareId), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IHardwareEntity>(null))
                .Verifiable();

            mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<HardwareEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .Verifiable();

            //act
            IHardwareService service = new HardwareService(mockRepository.Object, mockFloorRepository.Object);
            var updated = await service.ImmobilizeAsync(floorId, hardwareId);

            //assert
            updated.Should().BeFalse();

            mockRepository
                .Verify(r => r.GetByIdAsync(It.Is<string>(s => s == hardwareId), It.IsAny<CancellationToken>()), Times.Once());

            mockFloorRepository
                .Verify(r => r.GetByIdAsync(It.Is<string>(s => s == floorId), It.IsAny<CancellationToken>()), Times.Once());

            mockRepository
                .Verify(r => r.UpdateAsync(It.IsAny<HardwareEntity>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Trait(nameof(HardwareService), nameof(IHardwareService.ImmobilizeAsync))]
        [Fact]
        public async Task Given_Call_ImmobilizedAsync_When_Floor_Not_Exists_In_Repo_Should_Returns_Not_Updated()
        {
            //arrange
            var hardwareId = Guid.NewGuid().ToString();
            var floorId = Guid.NewGuid().ToString();
            Mock<IFloorRepository> mockFloorRepository = new();
            Mock<IHardwareRepository> mockRepository = new();

            mockFloorRepository
                .Setup(r => r.GetByIdAsync(It.Is<string>(s => s == floorId), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<FloorEntity>(null))
                .Verifiable();

            mockRepository
                .Setup(r => r.GetByIdAsync(It.Is<string>(s => s == hardwareId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HardwareEntity
                {
                    Id = hardwareId
                })
                .Verifiable();

            mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<HardwareEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .Verifiable();

            //act
            IHardwareService service = new HardwareService(mockRepository.Object, mockFloorRepository.Object);
            var updated = await service.ImmobilizeAsync(floorId, hardwareId);

            //assert
            updated.Should().BeFalse();

            mockRepository
                .Verify(r => r.GetByIdAsync(It.Is<string>(s => s == hardwareId), It.IsAny<CancellationToken>()), Times.Once());

            mockFloorRepository
                .Verify(r => r.GetByIdAsync(It.Is<string>(s => s == floorId), It.IsAny<CancellationToken>()), Times.Once());

            mockRepository
                .Verify(r => r.UpdateAsync(It.IsAny<HardwareEntity>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Trait(nameof(HardwareService), nameof(IHardwareService.ImmobilizeAsync))]
        [Fact]
        public async Task Given_Call_ImmobilizedAsync_When_Hardware_And_Floor_Exist_In_Repo_Should_Returns_Updated()
        {
            //arrange
            var hardwareId = Guid.NewGuid().ToString();
            var floorId = Guid.NewGuid().ToString();
            Mock<IFloorRepository> mockFloorRepository = new();
            Mock<IHardwareRepository> mockRepository = new();

            mockFloorRepository
                .Setup(r => r.GetByIdAsync(It.Is<string>(s => s == floorId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FloorEntity
                {
                    Id = floorId
                })
                .Verifiable();

            mockRepository
                .Setup(r => r.GetByIdAsync(It.Is<string>(s => s == hardwareId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HardwareEntity
                {
                    Id = hardwareId
                })
                .Verifiable();

            mockRepository
                .Setup(r => r.UpdateAsync(It.Is<ImmobilizedHardwareEntity>(h => h.Id == hardwareId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .Verifiable();

            //act
            IHardwareService service = new HardwareService(mockRepository.Object, mockFloorRepository.Object);
            var updated = await service.ImmobilizeAsync(floorId, hardwareId);

            //assert
            updated.Should().BeTrue();

            mockRepository
                .Verify(r => r.GetByIdAsync(It.Is<string>(s => s == hardwareId), It.IsAny<CancellationToken>()), Times.Once());

            mockFloorRepository
                .Verify(r => r.GetByIdAsync(It.Is<string>(s => s == floorId), It.IsAny<CancellationToken>()), Times.Once());

            mockRepository
                .Verify(r => r.UpdateAsync(It.Is<ImmobilizedHardwareEntity>(h => h.Id == hardwareId), It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
