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

namespace Imobilizados.Test
{
    public class HardwareControllerTest
    {
        [Fact]
        public async void Add_PassBlankDto_RespondsBadRequest_ReturnTrue()
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.AddAsync(It.IsAny<HardwareDto>()))
                .Callback((HardwareDto dto) =>
                { 
                    Console.WriteLine($"{dto.Id}");
                })
                .Returns(Task.FromResult(0));


            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.Create(new HardwareDto());

            Assert.NotNull(response);
            var expectedType = typeof(BadRequestResult);            
            Assert.IsType(expectedType, response);
        }


        [Theory]
        [InlineData("1gt2g3i5n6o3nh1l2")]
        public async void GetById_RespondsNotFound_ReturnsTrue(string value)
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
        public async void GetById_PassInvalidId_RespondsBadRequest_ReturnsTrue(string id)
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
        public async void GetById_RespondsOk_WithBody_ReturnsTrue(string id)
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
        public async void GetById_PassValidId_RespondsOk_WithBody_ReturnsTrue(string value)
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
        public async void GetById_RespondsOk_ReturnsTrue(string value)
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
        public async void LoadAllImmobilized_RespondsOk_WithEmptyList_ReturnsTrue()
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.LoadAllAsync())
                .ReturnsAsync(new List<HardwareDto> { });
            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.LoadAllAsync();

            Assert.NotNull(response);
            var expectedType = typeof(OkObjectResult);
            
            Assert.IsType(expectedType, response);
            
            var responseOk = response as OkObjectResult;
            Assert.NotNull(responseOk.Value);
            var responseCollection = responseOk.Value as IEnumerable<HardwareDto>;
            Assert.Empty(responseCollection);
        }

        [Fact]
        public async void LoadAllImmobilized_RespondsOk_WithNullReference_ReturnsTrue()
        {
            var mockService = new Mock<IHardwareService>();
            mockService.Setup( s => s.LoadAllAsync())
                .ReturnsAsync((List<HardwareDto>)null);
            var service = mockService.Object;
            var controller = new HardwareController(service);
            var response = await controller.LoadAllAsync();
            Assert.NotNull(response);
            var expectedType = typeof(OkObjectResult);
            
            Assert.IsType(expectedType, response);
            
            var responseOk = response as OkObjectResult;
            Assert.Null(responseOk.Value);
        }

        [Fact]
        public async void LoadAllImmobilized_RespondsOk_ReturnsTrue()
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
            var response = await controller.LoadAllAsync();
            
            Assert.NotNull(response);
            var expectedType = typeof(OkObjectResult);            
            Assert.IsType(expectedType, response);
        }

        [Fact]
        public async void LoadAllImmobilized_RespondsOk_WithFourElement_ReturnsTrueAndFour()
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
            var response = await controller.LoadAllAsync();            
            
            Assert.NotNull(response);
            var expectedType = typeof(OkObjectResult);
            
            Assert.IsType(expectedType, response);
            
            var responseOk = response as OkObjectResult;
            Assert.NotNull(responseOk.Value);
            var responseCollection = responseOk.Value as IEnumerable<HardwareDto>;
            Assert.NotEmpty(responseCollection);
        }
    }
}
