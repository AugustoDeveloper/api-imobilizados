using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Imobilizados.Application.Dtos
{
    public class FloorDto : IDto
    {
        #region 'Properties'
        
        [Required]
        [JsonProperty(PropertyName = "level")]
        public int Level { get; set; } = -1;

        [JsonProperty(PropertyName = "level_name")]
        public string LevelName { get; set; }

        #endregion
    }
}
