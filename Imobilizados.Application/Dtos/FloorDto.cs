using System;
using System.ComponentModel.DataAnnotations;

namespace Imobilizados.Application.Dtos
{
    public class FloorDto : IDto
    {
        #region 'Properties'

        [Display(Name = "id")]
        [Key]
        public string Id { get; set; }
        
        [Required]
        [Display(Name = "level")]
        public int Level { get; set; }

        [Display(Name = "level_name")]
        public string LevelName { get; set; }

        #endregion
    }
}
