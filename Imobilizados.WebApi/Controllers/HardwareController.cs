using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Imobilizados.Application.Dtos;
using Imobilizados.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Imobilizados.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ValidateModel]
    public class HardwareController : Controller
    {
        private IHardwareService _service;

        public HardwareController(IHardwareService service)
        {
            _service = service;
        }

        // GET api/values
        [HttpGet("/all")]
        public async Task<IActionResult> LoadAllAsync()
        {
            var collection = await _service.LoadAllAsync();
            return Ok(collection);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([Required] string id)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id)) return BadRequest(ModelState);

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

            return NoContent();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
