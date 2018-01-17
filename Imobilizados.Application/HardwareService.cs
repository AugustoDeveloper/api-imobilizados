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

        public override Task<Hardware> AddAsync(Hardware entity)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> DeleteAsync(Expression<Hardware> criteria)
        {
            throw new NotImplementedException();
        }

        public override Task<Hardware> GetByIdAsync(dynamic id)
        {
            throw new NotImplementedException();
        }

        public override Task<List<Hardware>> LoadAllAsync()
        {
            throw new NotImplementedException();
        }

        public override Task<List<Hardware>> LoadByAsync(Expression<Hardware> criteria)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> UpdateAsync(Hardware entity)
        {
            throw new NotImplementedException();
        }
    }
}
