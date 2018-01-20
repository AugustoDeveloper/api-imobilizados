using System;
using System.Threading.Tasks;
using Imobilizados.Application.Interfaces;
using Imobilizados.Domain.Repository;
using Moq;
using Xunit;
using System.Collections.Generic;
using Imobilizados.Application.Dtos;
using Imobilizados.WebApi.Controllers;
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
            var expectedType = typeof(BadRequestResult);
            
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
            var expectedType = typeof(BadRequestResult);
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
            var expectedType = typeof(BadRequestResult);
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
            var expectedType = typeof(NotFoundResult);
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
            var expectedType = typeof(BadRequestResult);
            Assert.IsType(expectedType, response);
        }

        [Fact]
        public async void Add_ShouldReturnsCreateAtRoute_WhenPassValidDto_WithEmptyOptionalMembers()
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.AddAsync(It.IsAny<HardwareDto>()))
                .Returns(Task.FromResult(0));

            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.Create(
                new HardwareDto
                {
                    Name = "Computador"
                }
            );            

            
            Assert.NotNull(response);
            var expectedType = typeof(CreatedAtRouteResult);
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
                .Returns(Task.FromResult(0));


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
                .Returns(Task.FromResult(0));


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
                .Returns(Task.FromResult(0));


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
                .Returns(Task.FromResult(0));


            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.Create(addHardware);
            

            Assert.NotNull(response);
            var expectedType = typeof(CreatedAtRouteResult);            
            Assert.IsType(expectedType, response);
        }

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
            var expectedType = typeof(NotFoundResult);            
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
            var expectedType = typeof(BadRequestResult);            
            Assert.IsType(expectedType, response);                
        }
        
        [Theory]
        [InlineData("1gt2g3i5n6o3nh1l2")]
        public async void GetById_ShouldReturnsStatusCodeOk_And_ResponseBodyIsNotNull_WhenPassValidId(string id)
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.GetByIdAsync(It.IsAny<string>()))            
            .ReturnsAsync(new HardwareDto());

            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.GetById(id);

            Assert.NotNull(response);
            var expectedType = typeof(OkObjectResult);            
            Assert.IsType(expectedType, response);
            
            var responseOk = response as OkObjectResult;
            Assert.NotNull(responseOk.Value);            
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
        
        [Theory]
        [InlineData("1a24dq12d3rf1d")]
        public async void GetById_ShouldReturnsStatusCodeOk_WhenPassValidId(string value)
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
        }

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
}
