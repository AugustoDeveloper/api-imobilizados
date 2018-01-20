using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Imobilizados.Application.Dtos;
using Imobilizados.Application.Interfaces;
using Imobilizados.Domain.Entity;
using Imobilizados.Domain.Repository;

namespace Imobilizados.Application
{
    public class HardwareService : BaseService<HardwareDto, Hardware, IHardwareRepository>, IHardwareService
    {
        public HardwareService(IHardwareRepository repository) : base(repository) { }

        public async Task<List<HardwareDto>> LoadByIsImmobilizedAsync(bool isImmobilized)
        {
            var collection = await Repository.LoadByIsImmobilizedAsync(isImmobilized);
            var collectioDto = TransformToDto(collection);
            return collectioDto;
        }

        public async Task<List<HardwareDto>> LoadByFloorAsync(FloorDto floorDto)
        {
            var floor = TransformToEntity<Floor, FloorDto>(floorDto);
            var collection = await Repository.LoadByFloorAsync(floor);
            var collectioDto = TransformToDto(collection);
            return collectioDto;
        }
    }
}
