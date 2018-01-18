using System;

namespace Imobilizados.Application.Dtos
{
    public class FloorDto : IDto
    {
        #region 'Properties'

        public object Id { get; set; }
        public int Level { get; set; }
        public string LevelName { get; set; }

        #endregion
    }
}
