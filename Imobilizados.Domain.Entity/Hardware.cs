using System;

namespace Imobilizados.Domain.Entity
{
    public class Hardware : IEntity
    {
        #region 'Properties'

        public dynamic Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string FacoryCode { get; set; }
        public string Description { get; set; }
        public bool IsImmobilized => ImmobilizerFloor != null && ImmobilizerFloor.Level > -1;
        public Floor ImmobilizerFloor { get; set; }

        #endregion
    }
}
