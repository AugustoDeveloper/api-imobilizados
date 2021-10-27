using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Imobilizados.Domain.Repositories
{
    public interface IRepository {  }
    public interface IRepository<TEntity> : IRepository 
    {
        Task<string> InsertAsync(TEntity entity, CancellationToken cancellationTokne = default);
        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TEntity> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    }
}
