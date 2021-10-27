using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Imobilizados.Application.Services.Interfaces;
using Imobilizados.Application.DTOs;
using Imobilizados.Application.Extensions;
using Imobilizados.Domain.Repositories;
using ImmobilizedHardwareEntity = Imobilizados.Domain.Entities.ImmobilizedHardware;

namespace Imobilizados.Application.Services
{
    public class FloorService : IFloorService
    {
        private readonly IFloorRepository floorRepository;
        private readonly IHardwareRepository hardwareRepository;

        public FloorService(IFloorRepository floorRepository, IHardwareRepository hardwareRepository)
        {
            this.floorRepository = floorRepository ?? throw new ArgumentNullException(nameof(floorRepository));
            this.hardwareRepository = hardwareRepository ?? throw new ArgumentNullException(nameof(hardwareRepository));
        }

        async Task<FloorCollection> IFloorService.GetAllAsync(CancellationToken cancellationToken = default)
        {
            var floors = await this.floorRepository.GetAllAsync(cancellationToken);
            return floors?.ToDTO() ?? FloorCollection.Empty;
        }

        async Task<Floor> IFloorService.GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var floor = await this.floorRepository.GetByIdAsync(id, cancellationToken);
            return floor?.ToDTO();
        }

        async Task<Floor> IFloorService.InsertAsync(Floor floor, CancellationToken cancellationToken = default)
        {
            var newFloor = await this.floorRepository.GetByLevelAsync(floor.Level, floor.LevelName, cancellationToken);

            if (newFloor == null)
            {
                var newFloorId = await this.floorRepository.InsertAsync(floor.ToEntity(), cancellationToken);
                newFloor = await this.floorRepository.GetByIdAsync(newFloorId, cancellationToken);
            }

            return newFloor.ToDTO();
        }

        async Task IFloorService.DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var hardwares = await this.hardwareRepository.GetAllImmobilizedAtFloorAsync(id, cancellationToken);

            if (hardwares.Any())
            {
                foreach(var hardware in hardwares)
                {
                    hardware.Floor = null;
                    await this.hardwareRepository.UpdateAsync(hardware);
                }
            }

            await  this.floorRepository.DeleteAsync(id, cancellationToken);
        }
    }
}
