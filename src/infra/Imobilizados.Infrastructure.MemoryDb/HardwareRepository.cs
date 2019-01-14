using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Imobilizados.Domain.Entity;
using Imobilizados.Domain.Repository;
using Imobilizados.Infrastructure.MemoryDb.Base;

namespace Imobilizados.Infrastructure.MemoryDb
{
    public class HardwareRepository : MemoryRepository<Hardware>, IHardwareRepository
    {
        public Task<List<Hardware>> LoadByFloorAsync(Floor floor)
        {
            return Task.FromResult(Database.Values.Where(h => h.ImmobilizerFloor?.Level == floor.Level).ToList());
        }

        public Task<List<Hardware>> LoadByIsImmobilizedAsync(bool isImmobilized)
        {
            return Task.FromResult(Database.Values.Where(h => h.IsImmobilized == isImmobilized ).ToList());
        }
    }
}