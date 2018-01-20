using System.Collections.Generic;
using System.Threading.Tasks;
using Imobilizados.Application.Dtos;
using Imobilizados.Domain.Entity;
using Imobilizados.Domain.Repository;

namespace Imobilizados.Application.Interfaces
{
    public interface IHardwareService : IService<HardwareDto, Hardware, IHardwareRepository>
    {
        Task<List<HardwareDto>> LoadByIsImmobilizedAsync(bool isImmobilized);
        Task<List<HardwareDto>> LoadByFloorAsync(FloorDto floorDto);
    }    
}