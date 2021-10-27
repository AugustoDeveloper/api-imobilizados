using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using LiteDB;
using Imobilizados.Domain.Entities;
using Imobilizados.Infrastructure.Repository.LiteDB.Base;
using Imobilizados.Domain.Repositories;

namespace Imobilizados.Infrastructure.Repository.LiteDB
{
    public sealed class FloorRepository : LiteDatabaseRepositoryBase<Floor>, IFloorRepository
    {
        public FloorRepository(ILiteDatabase database) : base(database, "floors") { }

        Task<Floor> IFloorRepository.GetByLevelAsync(int level, string andLevelName, CancellationToken cancellationToken = default)
        {
            return BuildTaskCompletionSource<Floor>(() => GetByLevel(level, andLevelName), cancellationToken);

            Floor GetByLevel(int level, string andLevelName)
            {
                Floor floor = null;
                var collection = GetCollection();
                var document = collection.FindOne($"$.level = {level} AND $.level_name = '{andLevelName}'");

                if (document != null)
                {
                    floor = ToEntity<Floor>(document);
                }

                return floor;
            }
        }

    }
}
