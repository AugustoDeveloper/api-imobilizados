using System;
using System.Threading.Tasks;
using Imobilizados.Application;
using Imobilizados.Application.Dtos;
using Imobilizados.Application.Interfaces;
using Imobilizados.Domain.Entity;
using Imobilizados.Domain.Repository;
using Imobilizados.Infrastructure.MongoDb;
using Imobilizados.WebApi;
using MongoDB.Driver;
using Xunit;

namespace Imobilizados.Test
{
    public class HardwareRepositoryTest
    {
        private static IMongoClient _client;
        private IHardwareRepository _repository;
        private IHardwareService _service;
        
        static HardwareRepositoryTest()
        {
            _client = new MongoClient("mongodb://127.0.0.1:27017/");            
        }

        public HardwareRepositoryTest()
        {
            MongoDbConfiguration.Map();
            AutoMapperConfiguration.Configure();
            _repository = new HardwareRepository(_client);
            _service = new HardwareService(_repository);
        }


        [Fact]
        public async void AddAsync_ShouldReturnsId_WhenInsertHardware()
        {
            var hardware = new HardwareDto {Name = "Teste"};
            hardware = await _service.AddAsync(hardware);
            Assert.NotNull(hardware.Id);
            
            Console.WriteLine(hardware.Id);
        }
    }
}