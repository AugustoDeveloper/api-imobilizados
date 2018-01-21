using System;
using System.Threading.Tasks;
using Imobilizados.Application.Dtos;
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
        
        static HardwareRepositoryTest()
        {
            _client = new MongoClient("mongodb://127.0.0.1:27017/");
        }

        public HardwareRepositoryTest()
        {
            _repository = new HardwareRepository(_client);
        }


        public async void GetDatabase_ShouldReturnInstanceDatabase()
        {
            var collecion = await _repository.LoadAllAsync();
            Assert.NotNull(collecion);
        }
    }
}