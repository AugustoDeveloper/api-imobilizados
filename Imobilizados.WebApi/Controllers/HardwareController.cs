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

        // GET api/values
        [HttpGet("/all")]
        public async Task<List<HardwareDto>> LoadAll()
        { 
            return await _service.LoadAllAsync();
        }

        [HttpGet("{id}")]
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

            return CreatedAtRoute("", new {id = dto.Id}, dto);
        }

        
        [HttpPut("{id}")]
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

        // DELETE api/values/5
        [HttpDelete("{id}")]
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
    }
}
