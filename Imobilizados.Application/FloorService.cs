using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Imobilizados.Application.Dtos;
using Imobilizados.Application.Interfaces;
using Imobilizados.Domain.Entity;
using Imobilizados.Domain.Repository;

namespace Imobilizados.Application
{

    public class FloorService : BaseService<FloorDto, Floor, IFloorRepository>, IFloorService
    {
        public FloorService(IFloorRepository repository) : base(repository)
        {
        }

        public override Task<Floor> AddAsync(Floor entity)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> DeleteAsync(Expression<Floor> criteria)
        {
            throw new NotImplementedException();
        }

        public override Task<Floor> GetByIdAsync(dynamic id)
        {
            throw new NotImplementedException();
        }

        public override Task<List<Floor>> LoadAllAsync()
        {
            throw new NotImplementedException();
        }

        public override Task<List<Floor>> LoadByAsync(Expression<Floor> criteria)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> UpdateAsync(Floor entity)
        {
            throw new NotImplementedException();
        }
    }
}
