using System;

namespace Imobilizados.Domain.Entity
{
    public class Hardware : IEntity
    {
        #region 'Properties'

        public string Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string FacoryCode { get; set; }
        public string Description { get; set; }
        public bool IsImmobilized { get; set; }
        public Floor ImmobilizerFloor { get; set; }

        #endregion
    }
}
