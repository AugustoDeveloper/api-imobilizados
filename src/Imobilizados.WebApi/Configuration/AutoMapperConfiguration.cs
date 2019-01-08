using AutoMapper;
using Imobilizados.Application.Dtos;
using Imobilizados.Domain.Entity;
using Microsoft.Extensions.DependencyInjection;

namespace Imobilizados.WebApi
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Hardware, HardwareDto>();
                cfg.CreateMap<Floor, FloorDto>();
                cfg.CreateMap<HardwareDto, Hardware>();
                cfg.CreateMap<FloorDto, Floor>();
            });
        }
    }    
}