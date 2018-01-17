using Imobilizados.Application.Dtos;
using Imobilizados.Domain.Entity;
using Imobilizados.Domain.Repository;

namespace Imobilizados.Application.Interfaces
{
    public interface IHardwareService : IService<HardwareDto, Hardware, IHardwareRepository> { }    
}