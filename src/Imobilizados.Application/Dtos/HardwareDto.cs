using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Imobilizados.Application.Dtos
{
    public class HardwareDto : IDto
    {
        #region 'Properties'

        [JsonProperty( PropertyName = "id")]
        [Key]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        [Required]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "brand")]
        public string Brand { get; set; }

        [JsonProperty(PropertyName = "factory_code")]
        public string FacoryCode { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "is_immobilized")]
        public bool IsImmobilized => ImmobilizerFloor?.Level > -1;

        [JsonProperty(PropertyName = "immobilizer_floor")]
        public FloorDto ImmobilizerFloor { get; set; }

        #endregion
    }
}
