using System.Threading.Tasks;
using System.Threading;
using Imobilizados.Application.DTOs;

namespace Imobilizados.Application.Services.Interfaces
{
    public interface IHardwareService
    {
        Task<HardwareCollection> GetAllAsync(bool isImobilized = false, CancellationToken cancellationToken = default);
        Task<Hardware> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<Hardware> InsertAsync(Hardware hardware, CancellationToken cancellationToken = default);
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(string id, Hardware hardware, CancellationToken cancellationToken = default);
        Task<bool> ImmobilizeAsync(string floorId, string hardwareId, CancellationToken cancellationToken = default);
    }
}
