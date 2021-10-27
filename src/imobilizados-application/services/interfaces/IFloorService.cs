using System.Threading.Tasks;
using System.Threading;
using Imobilizados.Application.DTOs;

namespace Imobilizados.Application.Services.Interfaces
{
    public interface IFloorService
    {
        Task<FloorCollection> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Floor> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<Floor> InsertAsync(Floor floor, CancellationToken cancellationToken = default);
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}
