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
        public async void Add_ShouldReturnsBadRequest_WhenPassBlankDto()
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
            var expectedType = typeof(BadRequestObjectResult);            
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
        public async void GetById_ShouldReturnsStatusCodeOk_And_ResponseBodyIsNotNul_And_DtoIdEqualsRequestId_WhenPassValidId(string value)
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
        public async void LoadAllImmobilized_ShouldReturnsStatusCodeOk_WhenServiceReturnsEmptyList()
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
        public async void LoadAllImmobilized_ShouldReturnsStatusCodeOk_WhenServiceReturnEmpyOrNullList()
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
        public async void LoadAllImmobilized_ShouldReturnsStatusCodeOk_WhenServiceReturnFullList()
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
        public async void LoadAllImmobilized_ShouldReturnsStatusCodeOk_And_ResponseBodyNotNull_And_ResponseBodyWithFourElements()
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
