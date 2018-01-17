using Imobilizados.Application.Dtos;
using Imobilizados.Domain.Entity;
using Imobilizados.Domain.Repository;

namespace Imobilizados.Application.Interfaces
{
    public interface IFloorService : IService<FloorDto, Floor, IFloorRepository> { }    
}