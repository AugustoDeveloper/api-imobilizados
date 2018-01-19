using System;
using System.ComponentModel.DataAnnotations;

namespace Imobilizados.Application.Dtos
{
    public class HardwareDto : IDto
    {
        #region 'Properties'

        [Display(Name = "id")]
        [Required]
        public string Id { get; set; }

        [Display(Name = "name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "brand")]
        public string Brand { get; set; }

        [Display(Name = "factory_code")]
        public string FacoryCode { get; set; }

        [Display(Name = "description")]
        public string Description { get; set; }

        [Display(Name = "is_immobilized")]
        public bool IsImmobilized { get; set; }

        [Display(Name = "immobilizer_floor")]
        public FloorDto ImmobilizerFloor { get; set; }

        #endregion
    }
}
