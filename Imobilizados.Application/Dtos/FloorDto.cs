using System;
using System.ComponentModel.DataAnnotations;

namespace Imobilizados.Application.Dtos
{
    public class FloorDto : IDto
    {
        #region 'Properties'
        
        [Required]
        [Display(Name = "level")]
        public int Level { get; set; } = -1;

        [Display(Name = "level_name")]
        public string LevelName { get; set; }

        #endregion
    }
}
