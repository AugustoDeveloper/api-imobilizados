using System;
using System.Threading.Tasks;
using Imobilizados.Application.Interfaces;
using Imobilizados.Domain.Repository;
using Moq;
using Xunit;
using System.Collections.Generic;
using Imobilizados.Application.Dtos;
using Imobilizados.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Imobilizados.Test
{
    public class HardwareControllerTest
    {
        private ActionContext _context;

        public HardwareControllerTest()
        {
            
            _context = new ActionContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }
#region "Immobilize"

        [Theory]
        [InlineData("1223123", 1)]
        public async void Immobilize_ShouldReturnsNotFound_WhenPassFloorDtoWithValidLevel_And_ServiceFoundHardware_And_HardwareNotImmobilized(string id, int level)
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new HardwareDto { Id = id, Name = "Computador" });
            mockService.Setup( s => s.UpdateAsync(It.IsAny<string>(), It.IsAny<HardwareDto>()))
                .Returns(Task.FromResult(0));

            var service = mockService.Object;
            var controller = new HardwareController(service);
            
            var response = await controller.Immobilize(id, new FloorDto {Level = level, LevelName = "Finance" });

            Assert.NotNull(response);
            var expectedType = typeof(NoContentResult);
            Assert.IsType(expectedType, response);
        }

        [Theory]
        [InlineData("1223123", 1)]
        public async void Immobilize_ShouldReturnsNotFound_WhenPassFloorDtoWithValidLevel_And_ServiceFoundHardware_And_HardwareAlreadyImmbilized(string id, int level)
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new HardwareDto { Id = id, Name = "Computador", ImmobilizerFloor = new FloorDto{ Level = 10 }});
            mockService.Setup( s => s.UpdateAsync(It.IsAny<string>(), It.IsAny<HardwareDto>()))
                .Returns(Task.FromResult(0));

            var service = mockService.Object;
            var controller = new HardwareController(service);
            
            var response = await controller.Immobilize(id, new FloorDto {Level = level, LevelName = "Finance" });

            Assert.NotNull(response);
            var expectedType = typeof(BadRequestObjectResult);
            Assert.IsType(expectedType, response);
        }

        [Theory]
        [InlineData("1223123", 1)]
        public async void Immobilize_ShouldReturnsNotFound_WhenPassFloorDtoWithValidLevel_And_ServiceNotFoundHardware(string id, int level)
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((HardwareDto)null);
            mockService.Setup( s => s.UpdateAsync(It.IsAny<string>(), It.IsAny<HardwareDto>()))
                .Returns(Task.FromResult(0));

            var service = mockService.Object;
            var controller = new HardwareController(service);
            
            var response = await controller.Immobilize(id, new FloorDto {Level = level, LevelName = "Finance" });

            Assert.NotNull(response);
            var expectedType = typeof(NotFoundObjectResult);
            Assert.IsType(expectedType, response);
        }

        [Theory]
        [InlineData("1223123", -1)]
        public async void Immobilize_ShouldReturnsBadRequest_WhenPassFloorDtoWithInvalidLevel(string id, int level)
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new HardwareDto());
            mockService.Setup( s => s.UpdateAsync(It.IsAny<string>(), It.IsAny<HardwareDto>()))
                .Returns(Task.FromResult(0));

            var service = mockService.Object;
            var controller = new HardwareController(service);
            
            var response = await controller.Immobilize(id, new FloorDto {Level = level});

            Assert.NotNull(response);
            var expectedType = typeof(BadRequestObjectResult);
            Assert.IsType(expectedType, response);
        }

        [Theory]
        [InlineData("1223123")]
        public async void Immobilize_ShouldReturnsBadRequest_WhenPassEmptyFloorDto(string id)
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new HardwareDto());
            mockService.Setup( s => s.UpdateAsync(It.IsAny<string>(), It.IsAny<HardwareDto>()))
                .Returns(Task.FromResult(0));

            var service = mockService.Object;
            var controller = new HardwareController(service);
            
            var response = await controller.Immobilize(id, new FloorDto());

            Assert.NotNull(response);
            var expectedType = typeof(BadRequestObjectResult);
            Assert.IsType(expectedType, response);
        }

        [Theory]
        [InlineData("1223123")]
        public async void Immobilize_ShouldReturnsBadRequest_WhenPassNullFloorDto(string id)
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new HardwareDto());
            mockService.Setup( s => s.UpdateAsync(It.IsAny<string>(), It.IsAny<HardwareDto>()))
                .Returns(Task.FromResult(0));

            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.Immobilize(id, new FloorDto());

            Assert.NotNull(response);
            var expectedType = typeof(BadRequestObjectResult);
            Assert.IsType(expectedType, response);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public async void Immobilize_ShouldReturnsBadRequest_WhenPassInvalidId(string id)
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new HardwareDto());
            mockService.Setup( s => s.UpdateAsync(It.IsAny<string>(), It.IsAny<HardwareDto>()))
                .Returns(Task.FromResult(0));

            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.Immobilize(id, new FloorDto());

            Assert.NotNull(response);
            var expectedType = typeof(BadRequestObjectResult);
            Assert.IsType(expectedType, response);
        }

#endregion

#region "LoadByFloor"

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void LoadByFloor_ShouldReturnsResponseBodyWithTwoElementInList_And_ElementsIsImmobilized_And_ImmobilizerFloorContainsLevelEqualFloorLevelParameter_WhenPassFloorLevelValid_And_ServiceReturnFullList(int floorLevel)
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.LoadByFloorAsync(It.IsAny<FloorDto>()))
                .ReturnsAsync((FloorDto floor) => 
                {
                    floor.LevelName = "Infra";                    
                    return new List<HardwareDto>
                    {
                        new HardwareDto { Id = Guid.NewGuid().ToString(), Name = "Computador", ImmobilizerFloor = floor },
                        new HardwareDto { Id = Guid.NewGuid().ToString(), Name = "Computador", ImmobilizerFloor = floor }
                    };
                });
            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.LoadByFloor(floorLevel);
            
            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.Equal(2, response.Count);
            Assert.All(response, (element) => 
            {
                Assert.True(element.IsImmobilized);
                Assert.True(element.ImmobilizerFloor?.Level == floorLevel);
            });
        }

        [Theory]
        [InlineData(-1)]
        public async void LoadByFloor_ShouldReturnsResponseBodyWithEmptyList_WhenPassInvalidFloorLevel_And_ServiceReturnEmptyList(int floorLevel)
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.LoadByFloorAsync(It.IsAny<FloorDto>()))
                .ReturnsAsync(new List<HardwareDto>());
            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.LoadByFloor(floorLevel);
            Assert.NotNull(response);
            Assert.Empty(response);
        }

        [Theory]
        [InlineData(-1)]
        public async void LoadByFloor_ShouldReturnsResponseBodyWithNullList_WhenPassInvalidFloorLevel_And_ServiceReturnNullList(int floorLevel)
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.LoadByFloorAsync(It.IsAny<FloorDto>()))
                .ReturnsAsync((List<HardwareDto>)null);
            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.LoadByFloor(floorLevel);
            Assert.Null(response);
        }        
#endregion

#region "LoadByIsImmobilized"

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void LoadByIsImmobilized_ShouldReturnsResponseBodyWithTwoElementInList_WhenPassIsImmobilizedTrueOrFalse_And_ServiceReturnFullList(bool isImmobilized)
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.LoadByIsImmobilizedAsync(It.Is<bool>(c => c)))
                .ReturnsAsync((bool value) =>
                {
                    return new List<HardwareDto>
                    {
                        new HardwareDto
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = "Computador Lenovo",
                            ImmobilizerFloor = new FloorDto
                            {
                                Level = 1,
                                LevelName = "T.I"
                            }
                        },
                        new HardwareDto
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = "Computador HP",
                            ImmobilizerFloor = new FloorDto
                            {
                                Level = 1,
                                LevelName = "T.I"
                            }
                        }
                    };
                });
            mockService.Setup( s => s.LoadByIsImmobilizedAsync(It.Is<bool>(c => !c)))
                .ReturnsAsync((bool value) =>
                {   
                    return new List<HardwareDto>
                    {
                        new HardwareDto
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = "Mouse Apple"
                        },
                        new HardwareDto
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = "Mouse Dell"
                        }
                    };
                });
            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.LoadByIsImmobilized(isImmobilized);
            
            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.Equal(2, response.Count);
            Assert.All(response, (element) => Assert.Equal(isImmobilized, element.IsImmobilized));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void LoadByIsImmobilized_ShouldReturnsResponseBodyWithEmptyList_WhenPassIsImmobilizedTrueOrFalse_And_ServiceReturnEmptyList(bool isImmobilized)
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.LoadByIsImmobilizedAsync(It.IsAny<bool>()))
                .ReturnsAsync(new List<HardwareDto>());
            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.LoadByIsImmobilized(isImmobilized);
            
            Assert.NotNull(response);
            Assert.Empty(response);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void LoadByIsImmobilized_ShouldReturnsResponseBodyWithNullList_WhenPassIsImmobilizedTrueOrFalse_And_ServiceReturnNullList(bool isImmobilized)
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.LoadByIsImmobilizedAsync(It.IsAny<bool>()))
                .ReturnsAsync((List<HardwareDto>)null);
            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.LoadByIsImmobilized(isImmobilized);
            Assert.Null(response);
        }       
#endregion

#region "Delete"

        [Theory]
        [InlineData("123sdfsxcv")]
        public async void Delete_ShouldReturnsBadRequest_WhenPassValidId_And_ServiceFoundHardware(string id)
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new HardwareDto { Id = id});
            mockService.Setup( s => s.UpdateAsync(It.IsAny<string>(), It.IsAny<HardwareDto>()))
                .Returns(Task.FromResult(0));

            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.Delete(id);

            Assert.NotNull(response);
            var expectedType = typeof(NoContentResult);
            Assert.IsType(expectedType, response);
        }

        [Fact]
        public async void Delete_ShouldReturnsBadRequest_WhenPassValidId_And_ServiceNotFoundHardware()
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((HardwareDto)null);
            mockService.Setup( s => s.UpdateAsync(It.IsAny<string>(), It.IsAny<HardwareDto>()))
                .Returns(Task.FromResult(0));

            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.Delete(Guid.NewGuid().ToString());

            Assert.NotNull(response);
            var expectedType = typeof(NotFoundObjectResult);
            Assert.IsType(expectedType, response);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public async void Delete_ShouldReturnsBadRequest_WhenPassInvalidId(string id)
        {
            var mockService = new Mock<IHardwareService>();            
            mockService.Setup( s => s.UpdateAsync(It.IsAny<string>(), It.IsAny<HardwareDto>()))
                .Returns(Task.FromResult(0));

            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.Delete(id);

            Assert.NotNull(response);
            var expectedType = typeof(BadRequestObjectResult);
            Assert.IsType(expectedType, response);
        }

#endregion

#region "Update"
        [Theory]
        [InlineData("123456")]
        [InlineData("123askdnaks456")]
        [InlineData("123askdnak123jol")]
        public async void Update_ShouldReturnsNoContents_And_ReturnsResponseBodyNotNull_AndValidDtoEqualIdDtoIdParameter_WhenPassValidId_And_PassValidToDto(string idParameter)
        {
            var mockService = new Mock<IHardwareService>();
            
            mockService.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) => 
                new HardwareDto
                {
                    Id = idParameter,
                    Name = "Computador",
                    FacoryCode = Guid.NewGuid().ToString()                    
                });
            mockService.Setup( s => s.UpdateAsync(It.IsAny<string>(), It.IsAny<HardwareDto>()))
                .Returns(Task.FromResult(0));

            var service = mockService.Object;
            var controller = new HardwareController(service);

            var updatingDto = new HardwareDto
                {
                    Id = idParameter,
                    Name = "Computador",
                    FacoryCode = Guid.NewGuid().ToString()                    
                };
            var response = await controller.Update(idParameter, updatingDto);

            Assert.NotNull(response);            
            var expectedType = typeof(NoContentResult);
            Assert.IsType(expectedType, response);
        }

        [Theory]
        [InlineData("123456")]
        [InlineData("123askdnaks456")]
        [InlineData("123askdnak123jol")]
        public async void Update_ShouldReturnsBadRequest_WhenPassBlankDto_And_ValidId(string idParameter)
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.UpdateAsync(It.IsAny<string>(), It.IsAny<HardwareDto>()))
                .Returns(Task.FromResult(0));

            var service = mockService.Object;
            var controller = new HardwareController(service);
            
            controller.ModelState.AddModelError("Id", "Required");
            controller.ModelState.AddModelError("Name", "Required");

            var updatingDto = new HardwareDto();
            var response = await controller.Update(idParameter, updatingDto);
            Assert.NotNull(response);
            var expectedType = typeof(BadRequestObjectResult);
            
            Assert.IsType(expectedType, response);
        }
        
        [Theory]
        [InlineData("123456")]
        [InlineData("123askdnaks456")]
        [InlineData("123askdnak123jol")]
        public async void Update_ShouldReturnsStatusCodeOk_WhenPassNullDto(string idParameter)
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new HardwareDto());
            mockService.Setup( s => s.UpdateAsync(It.IsAny<string>(), It.IsAny<HardwareDto>()))
                .Returns(Task.FromResult(0));

            var service = mockService.Object;
            var controller = new HardwareController(service);
            
            controller.ModelState.AddModelError("Id", "Required");
            controller.ModelState.AddModelError("Name", "Required");

            var response = await controller.Update(idParameter, null);

            Assert.NotNull(response);
            var expectedType = typeof(BadRequestObjectResult);
            Assert.IsType(expectedType, response);            
        }

        [Theory]
        [InlineData("123456")]
        public async void Update_ShouldReturnsBadRequest_WhenPassIdParameterIsDiferenteDtoId(string idParameter)
        {
            var mockService = new Mock<IHardwareService>();

            mockService.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new HardwareDto());
                
            mockService.Setup( s => s.UpdateAsync(It.IsAny<string>(), It.IsAny<HardwareDto>()))
                .Returns(Task.FromResult(0));

            var service = mockService.Object;
            var controller = new HardwareController(service);
            
            controller.ModelState.AddModelError("Id", "Required");
            controller.ModelState.AddModelError("Name", "Required");

            var response = await controller.Update(idParameter,
                new HardwareDto
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Computador"
                }
            );

            Assert.NotNull(response);
            var expectedType = typeof(BadRequestObjectResult);
            Assert.IsType(expectedType, response);
        }
        
        [Theory]
        [InlineData("123456")]
        public async void Update_ShouldReturnsBadRequest_WhenPassValidId_And_ServiceNotFoundHardware(string idParameter)
        {
            var mockService = new Mock<IHardwareService>();
            
            mockService.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((HardwareDto)null);

            mockService.Setup( s => s.UpdateAsync(It.IsAny<string>(), It.IsAny<HardwareDto>()))
                .Returns(Task.FromResult(0));

            var service = mockService.Object;
            var controller = new HardwareController(service);

            var response = await controller.Update(idParameter,
                new HardwareDto
                {
                    Id = idParameter,
                    Name = "Computador"
                }
            );

            Assert.NotNull(response);
            var expectedType = typeof(NotFoundObjectResult);
            Assert.IsType(expectedType, response);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public async void Update_ShouldReturnsBadRequest_WhenPassInvalidId(string id)
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new HardwareDto());
            mockService.Setup( s => s.UpdateAsync(It.IsAny<string>(), It.IsAny<HardwareDto>()))
                .Returns(Task.FromResult(0));

            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.Update(id,
                new HardwareDto
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Computador"
                }
            );

            Assert.NotNull(response);
            var expectedType = typeof(BadRequestObjectResult);
            Assert.IsType(expectedType, response);
        }
#endregion

#region "Add"

        [Fact]
        public async void Add_ShouldReturnsCreateAtRoute_WhenPassValidDto_WithEmptyOptionalMembers()
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.AddAsync(It.IsAny<HardwareDto>()))
                .ReturnsAsync((HardwareDto dto) => dto);

            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.Create(
                new HardwareDto
                {
                    Name = "Computador"
                }
            );            

            
            Assert.NotNull(response);
            var expectedType = typeof(CreatedResult);
            Assert.IsType(expectedType, response);
        }        

        
        [Fact]
        public async void Add_ShouldReturnsBadRequest_WhenPassDtoWithId()
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.AddAsync(It.IsAny<HardwareDto>()))
                .Callback((HardwareDto dto) =>
                {
                    dto.Name = "";
                    dto.Id = Guid.NewGuid().ToString();
                    Console.WriteLine($"{dto.Id}");
                })
                .ReturnsAsync((HardwareDto dto) => dto);


            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.Create(new HardwareDto { Id = Guid.NewGuid().ToString()});            

            Assert.NotNull(response);
            var expectedType = typeof(BadRequestObjectResult);            
            Assert.IsType(expectedType, response);
        }

        [Fact]
        public async void Add_ShouldReturnsBadRequest_WhenPassNullDto()
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.AddAsync(It.IsAny<HardwareDto>()))
                .Callback((HardwareDto dto) =>
                {
                    dto.Name = "";
                    dto.Id = Guid.NewGuid().ToString();
                    Console.WriteLine($"{dto.Id}");
                })
                .ReturnsAsync((HardwareDto dto) => dto);


            var service = mockService.Object;
            var controller = new HardwareController(service);
            controller.ModelState.AddModelError("Id", "Required");
            controller.ModelState.AddModelError("Name", "Required");
            var response = await controller.Create(null);            

            Assert.NotNull(response);
            var expectedType = typeof(BadRequestObjectResult);            
            Assert.IsType(expectedType, response);
        }

        [Fact]
        public async void Add_ShouldReturnsBadRequest_WhenPassEmptyDto()
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.AddAsync(It.IsAny<HardwareDto>()))
                .Callback((HardwareDto dto) =>
                {
                    dto.Name = "";
                    dto.Id = Guid.NewGuid().ToString();
                    Console.WriteLine($"{dto.Id}");
                })
                .ReturnsAsync((HardwareDto dto) => dto);


            var service = mockService.Object;
            var controller = new HardwareController(service);
            controller.ModelState.AddModelError("Id", "Required");
            controller.ModelState.AddModelError("Name", "Required");
            var response = await controller.Create(new HardwareDto());            

            Assert.NotNull(response);
            var expectedType = typeof(BadRequestObjectResult);            
            Assert.IsType(expectedType, response);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async void Add_ShouldReturnsBadRequest_WhenPassInvalidNameToDto(string name)
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.AddAsync(It.IsAny<HardwareDto>()))
                .Callback((HardwareDto dto) =>
                {
                    dto.Name = name;
                    dto.Id = Guid.NewGuid().ToString();
                    Console.WriteLine($"{dto.Id}");
                })
                .ReturnsAsync((HardwareDto dto) => dto);


            var service = mockService.Object;
            var controller = new HardwareController(service);
            controller.ModelState.AddModelError("Name", "Required");
            var response = await controller.Create(new HardwareDto { Name = name });
            

            Assert.NotNull(response);
            var expectedType = typeof(BadRequestObjectResult);            
            Assert.IsType(expectedType, response);
        }

        [Fact]
        public async void Add_ShouldReturnsCreatedAtRouteResult_WhenPassValidDto()
        {
            var mockService = new Mock<IHardwareService>();
            var addHardware = new HardwareDto
            {
                Name = "Computador",
                Brand = "HP",
                Description = "2gb de ram",
                FacoryCode = "123123"
            };

            mockService.Setup( s => s.AddAsync(It.IsAny<HardwareDto>()))
                .Callback((HardwareDto dto) =>
                {                    
                    dto.Id = Guid.NewGuid().ToString();                    
                })
                .ReturnsAsync((HardwareDto dto) => dto);


            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.Create(addHardware);
            

            Assert.NotNull(response);
            var expectedType = typeof(CreatedResult);            
            Assert.IsType(expectedType, response);
        }
#endregion

#region "GetById"

        [Theory]
        [InlineData("1gt2g3i5n6o3nh1l2")]
        public async void GetById_ShouldReturnsRequestNotFound_WhenServiceReturnNull(string value)
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.GetByIdAsync(It.IsAny<string>()))            
            .ReturnsAsync((HardwareDto)null);

            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.GetById(value);

            Assert.NotNull(response);
            var expectedType = typeof(NotFoundObjectResult);            
            Assert.IsType(expectedType, response);                
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async void GetById_ShouldReturnsBadRequest_WhenIdRequestIsInvalid(string id)
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.GetByIdAsync(It.IsAny<string>()))            
            .ReturnsAsync(new HardwareDto());

            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.GetById(id);

            Assert.NotNull(response);
            var expectedType = typeof(BadRequestObjectResult);            
            Assert.IsType(expectedType, response);                
        }

        [Theory]
        [InlineData("1gt2g3i5n6o3nh1l2")]
        [InlineData("1gt2g3i112njiv2")]
        [InlineData("1gt1poxon6o3nh1l2")]
        [InlineData("1gt2g3i5n6123asc2")]
        public async void GetById_ShouldReturnsStatusCodeOk_And_ResponseBodyIsNotNull_And_DtoIdEqualsRequestId_WhenPassValidId(string value)
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) => new HardwareDto{ Id = id });

            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.GetById(value);

            Assert.NotNull(response);
            var expectedType = typeof(OkObjectResult);            
            Assert.IsType(expectedType, response);
            
            var responseOk = response as OkObjectResult;
            Assert.NotNull(responseOk.Value);
            var dto = responseOk.Value as HardwareDto;
            Assert.Equal(value, dto.Id);
        }

#endregion 

#region "LoadAdll"

        [Fact]
        public async void LoadAll_ShouldReturnsNullList_WhenServiceReturnNullList()
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.LoadAllAsync())
                .ReturnsAsync((List<HardwareDto>)null);
            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.LoadAll();
            Assert.Null(response);
        }

        [Fact]
        public async void LoadAll_ShouldReturnsEmptyList_WhenServiceReturnEmptyList()
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.LoadAllAsync())
                .ReturnsAsync(new List<HardwareDto>{ });
            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.LoadAll();
            
            Assert.NotNull(response);
            Assert.Empty(response);
        }

        [Fact]
        public async void LoadAll_ShouldReturnsResponseBodyWithFourElements_WhenServiceReturnListWithFourElements()
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.LoadAllAsync())
                .ReturnsAsync(new List<HardwareDto>{
                    new HardwareDto(),
                    new HardwareDto(),
                    new HardwareDto(),
                    new HardwareDto(),
                });
            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.LoadAll();            
            
            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.Equal(4, response.Count);
        }
    }

#endregion

}
