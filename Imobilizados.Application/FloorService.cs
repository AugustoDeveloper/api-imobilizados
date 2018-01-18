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
        public FloorService(IFloorRepository repository) : base(repository) { }
    }
}
