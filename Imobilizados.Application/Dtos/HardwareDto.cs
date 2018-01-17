using System;

namespace Imobilizados.Application.Dtos
{
    public class HardwareDto : IDto
    {
        #region 'Properties'

        public dynamic Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string FacoryCode { get; set; }
        public string Description { get; set; }
        public bool IsImmobilized => ImmobilizerFloor != null && ImmobilizerFloor.Level > -1;
        public FloorDto ImmobilizerFloor { get; set; }

        #endregion
    }
}
