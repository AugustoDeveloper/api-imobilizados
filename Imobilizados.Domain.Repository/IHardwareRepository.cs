using System.Collections.Generic;
using System.Threading.Tasks;
using Imobilizados.Domain.Entity;
using Imobilizados.Domain.Repository.Base;

namespace Imobilizados.Domain.Repository
{
    public interface IHardwareRepository : IRepository<Hardware>
    {
        Task<List<Hardware>> LoadByIsImmobilizedAsync(bool isImmobilized);
        Task<List<Hardware>> LoadByFloorAsync(Floor floor);

    }
}