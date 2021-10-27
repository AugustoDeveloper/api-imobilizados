using System.Collections.Generic;
using System.Linq;
using ImmobilizedHardwareEntity = Imobilizados.Domain.Entities.ImmobilizedHardware;
using HardwareDTO = Imobilizados.Application.DTOs.Hardware;
using HardwareCollectionDTO = Imobilizados.Application.DTOs.HardwareCollection;
using FloorCollectionDTO = Imobilizados.Application.DTOs.FloorCollection;
using IHardwareEntity = Imobilizados.Domain.Entities.IHardware;
using HardwareEntity = Imobilizados.Domain.Entities.Hardware;
using FloorEntity = Imobilizados.Domain.Entities.Floor;
using FloorDTO = Imobilizados.Application.DTOs.Floor;

namespace Imobilizados.Application.Extensions
{
    public static class MappingExtension
    {
        public static FloorCollectionDTO ToDTO(this List<FloorEntity> floors)
            => new FloorCollectionDTO(floors.Select(ToDTO).ToList());

        public static HardwareCollectionDTO ToDTO(this List<IHardwareEntity> hardwares)
            => new HardwareCollectionDTO(hardwares.Select(ToDTO).ToList());

        public static List<IHardwareEntity> ToEntity(this List<HardwareDTO> hardwares)
            => hardwares.Select(ToEntity).ToList();

        public static HardwareDTO ToDTO(this IHardwareEntity hardware)
            => hardware.IsImmobilized() 
            ? ToDTO((hardware as ImmobilizedHardwareEntity))
            : new HardwareDTO
            {
                Brand = hardware.Brand,
                Description = hardware.Description,
                FactoryCode = hardware.FactoryCode,
                Id = hardware.Id,
                Name = hardware.Name
            };

        public static HardwareDTO ToDTO(ImmobilizedHardwareEntity hardware)
            => new HardwareDTO
            {
                Brand = hardware.Brand,
                Description = hardware.Description,
                FactoryCode = hardware.FactoryCode,
                Id = hardware.Id,
                Name = hardware.Name,
                Floor = hardware.Floor.ToDTO(),
            };

        public static IHardwareEntity ToEntity(this HardwareDTO hardware)
            => new HardwareEntity
            {
                Brand = hardware.Brand,
                Description = hardware.Description,
                FactoryCode = hardware.FactoryCode,
                Name = hardware.Name
            };

        public static FloorDTO ToDTO(this FloorEntity floor)
            => new FloorDTO
            {
                Id = floor.Id,
                Level = floor.Level,
                LevelName = floor.LevelName
            };

        public static FloorEntity ToEntity(this FloorDTO floor)
            => new FloorEntity
            {
                Level = floor.Level,
                LevelName = floor.LevelName
            };
    }
}
