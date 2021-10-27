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
    public sealed class HardwareRepository : LiteDatabaseRepositoryBase<IHardware>, IHardwareRepository
    {
        public HardwareRepository(ILiteDatabase database) : base(database, "hardwares") { }

        Task<List<ImmobilizedHardware>> IHardwareRepository.GetAllImmobilizedAtFloorAsync(string floorId, CancellationToken cancellationToken = default)
        {
            return BuildTaskCompletionSource<List<ImmobilizedHardware>>(() => GetAllImmobilizedAtFloor(floorId), cancellationToken);

            List<ImmobilizedHardware> GetAllImmobilizedAtFloor(string floorId)
            {
                var typeName = typeof(ImmobilizedHardware).AssemblyQualifiedName;
                var collection = GetCollection();
                var documents = collection.Find($"_type = '{typeName}' and $.floor._id = '{floorId}'");

                var immobilizeds = documents.Select(ToEntity<ImmobilizedHardware>).ToList();
                return immobilizeds;
            }
        }

        Task<List<IHardware>> IHardwareRepository.GetAllImobilizedAsync(CancellationToken cancellationToken = default)
        {
            return BuildTaskCompletionSource<List<IHardware>>(() => GetAllImmobilized(), cancellationToken);

            List<IHardware> GetAllImmobilized()
            {
                var typeName = typeof(ImmobilizedHardware).AssemblyQualifiedName;
                var collection = GetCollection();
                var documents = collection.Find($"_type = '{typeName}'");

                var immobilizeds = documents.Select(ToEntity<ImmobilizedHardware>).ToList<IHardware>();
                return immobilizeds;
            }
        }
    }
}
