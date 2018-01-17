using System;

namespace Imobilizados.Application.Dtos
{
    public class FloorDto : IDto
    {
        #region 'Properties'

        public dynamic Id { get; set; }
        public int Level { get; set; }
        public string Departament { get; set; }

        #endregion
    }
}
