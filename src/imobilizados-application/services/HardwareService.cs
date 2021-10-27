using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Imobilizados.Application.Services.Interfaces;
using Imobilizados.Application.DTOs;
using Imobilizados.Application.Extensions;
using Imobilizados.Domain.Repositories;
using IHardwareEntity = Imobilizados.Domain.Entities.IHardware;
using ImmobilizedHardwareEntity = Imobilizados.Domain.Entities.ImmobilizedHardware;

namespace Imobilizados.Application.Services
{
    public class HardwareService : IHardwareService
    {
        private readonly IHardwareRepository repository;
        private readonly IFloorRepository floorRepository;

        public HardwareService(IHardwareRepository repository, IFloorRepository floorRepository)
        {
            this.repository =  repository ?? throw new ArgumentNullException(nameof(repository));
            this.floorRepository = floorRepository ?? throw new ArgumentNullException(nameof(floorRepository));
        }

        async Task<HardwareCollection> IHardwareService.GetAllAsync(bool onlyImmobilized = false, CancellationToken cancellationToken = default)
        {
            var hardwareEntities = onlyImmobilized 
                ? await this.repository.GetAllImobilizedAsync(cancellationToken)
                : await this.repository.GetAllAsync(cancellationToken);

            return hardwareEntities?.ToDTO() ?? HardwareCollection.Empty;
        }

        async Task<Hardware> IHardwareService.GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var hardwareEntity = await this.repository.GetByIdAsync(id, cancellationToken);

            return hardwareEntity?.ToDTO();
        }

        async Task<Hardware> IHardwareService.InsertAsync(Hardware hardware, CancellationToken cancellationToken = default)
        {
            var hardwareEntity = hardware.ToEntity();
            var id = await this.repository.InsertAsync(hardwareEntity, cancellationToken);
            hardwareEntity = await this.repository.GetByIdAsync(id);
            return hardwareEntity.ToDTO();
        }

        Task IHardwareService.DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            return this.repository.DeleteAsync(id, cancellationToken);
        }

        async Task<bool> IHardwareService.UpdateAsync(string id, Hardware hardware, CancellationToken cancellationToken = default)
        {
            var updatingHardwareEntity = hardware.ToEntity();
            var locateHardwareEntity = await this.repository.GetByIdAsync(id);

            if(locateHardwareEntity == null)
            {
                return false;
            }

            updatingHardwareEntity.Id = locateHardwareEntity.Id;
            var updated = await this.repository.UpdateAsync(updatingHardwareEntity, cancellationToken);
            return updated;
        }

        async Task<bool> IHardwareService.ImmobilizeAsync(string floorId, string hardwareId, CancellationToken cancellationToken = default)
        {
            var floor = await this.floorRepository.GetByIdAsync(floorId, cancellationToken);
            IHardwareEntity hardware = await this.repository.GetByIdAsync(hardwareId, cancellationToken);

            if (floor == null || hardware == null)
            {
                return false;
            }

            hardware = new ImmobilizedHardwareEntity(hardware, floor);

            var updated = await this.repository.UpdateAsync(hardware, cancellationToken);

            return updated;
        }
    }
}
