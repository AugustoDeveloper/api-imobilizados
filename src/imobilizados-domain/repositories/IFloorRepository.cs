using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Imobilizados.Domain.Entities;

namespace Imobilizados.Domain.Repositories
{
    public interface IFloorRepository : IRepository<Floor> 
    { 
        Task<Floor> GetByLevelAsync(int level, string andLevelName, CancellationToken cancellationToken = default);
    }
}
