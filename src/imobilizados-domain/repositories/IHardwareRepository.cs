using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Imobilizados.Domain.Entities;

namespace Imobilizados.Domain.Repositories
{
    public interface IHardwareRepository : IRepository<IHardware> 
    {
        Task<List<ImmobilizedHardware>> GetAllImmobilizedAtFloorAsync(string floorId, CancellationToken cancellationToken = default);
        Task<List<IHardware>> GetAllImobilizedAsync(CancellationToken cancellationToken = default);
    }
}
