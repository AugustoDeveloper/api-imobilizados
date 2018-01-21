using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Imobilizados.Application.Dtos;
using Imobilizados.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Imobilizados.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class HardwareController : Controller
    {
        private IHardwareService _service;

        public HardwareController(IHardwareService service)
        {
            _service = service;
        }
        
        [HttpGet("all")]
        public async Task<List<HardwareDto>> LoadAll()
        { 
            return await _service.LoadAllAsync();
        }

        [HttpGet("{id:length(24)}", Name = "GetById")]
        public async Task<IActionResult> GetById([Required] string id)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id)) 
            {
                return BadRequest();
            }

            var hardware = await _service.GetByIdAsync(id);
            if (hardware != null)
            {
                return Ok(hardware);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]HardwareDto dto)
        {
            if (dto == null || !ModelState.IsValid)  return BadRequest(ModelState);

            await _service.AddAsync(dto);

            return Created("/", dto);
        }

        
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update([Required]string id, [FromBody]HardwareDto dto)
        {
            if (string.IsNullOrEmpty(id) || 
                string.IsNullOrWhiteSpace(id) ||
                dto == null || 
                dto.Id != id ||
                !ModelState.IsValid) 
            {
                return BadRequest();
            }

            var existsDto = await _service.GetByIdAsync(id);
            if (existsDto == null) 
            {
                return NotFound();
            }

            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id) || 
                string.IsNullOrWhiteSpace(id)) 
            {
                return BadRequest();
            }

            var existsDto = await _service.GetByIdAsync(id);
            if (existsDto == null) 
            {
                return NotFound();
            }

            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("immobilized")]
        public async Task<List<HardwareDto>> LoadByIsImmobilized([FromQuery(Name = "is_immobilized")]bool isImmobilized = true)
        {
            return await _service.LoadByIsImmobilizedAsync(isImmobilized);
        }

        [HttpGet("immobilized/floor/{floorLevel}")]
        public async Task<List<HardwareDto>> LoadByFloor(int floorLevel)
        {
            return await _service.LoadByFloorAsync(new FloorDto { Level = floorLevel });
        }

        [HttpPut("immobilize/{id:length(24)}")]
        public async Task<IActionResult> Immobilize([Required]string id, [FromBody]FloorDto floor)
        {
            if (string.IsNullOrEmpty(id) || 
                string.IsNullOrWhiteSpace(id) ||
                (floor == null) ||
                (floor?.Level <  0)
                ) 
            {
                return BadRequest(new { message = "Invalid request" });
            }

            var existsDto = await _service.GetByIdAsync(id);
            if (existsDto == null) 
            {
                return NotFound(new { message = "Hardware not found" });
            }

            if (existsDto.IsImmobilized)
            {
                return BadRequest(new { message = "Hardware is already immobilized" });
            }

            existsDto.ImmobilizerFloor = floor;

            await _service.UpdateAsync(id, existsDto);
            return NoContent();            
        }
    }
}