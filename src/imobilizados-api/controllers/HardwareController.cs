using FluentValidation;
using Imobilizados.Application.DTOs;
using Imobilizados.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Imobilizados.API.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}")]
    [ ApiVersion("1")]
    public class HardwareController : ControllerBase
    {

        [HttpGet("hardwares/{id}")]
        public async Task<IActionResult> GetAsync(
            [FromRoute] string id,
            [FromServices] IHardwareService service,
            CancellationToken cancellationToken
        )
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var hardware = await service.GetByIdAsync(id, cancellationToken);
            if (hardware == null)
            {
                return NotFound();
            }

            return Ok(hardware);
        }

        [HttpGet("hardwares")]
        public async Task<IActionResult> GetAllAsync(
            [FromQuery(Name = "only_immobilized")] bool onlyImmobilized,
            [FromServices] IHardwareService service,
            CancellationToken cancellationToken
        )
        {
            var hardwares = await service.GetAllAsync(onlyImmobilized, cancellationToken);

            return Ok(hardwares);
        }

        [HttpPut("hardwares/{id}")]
        public async Task<IActionResult> PutAsync(
                [FromRoute] string id,
                [FromBody] Hardware hardware,
                [FromServices] IHardwareService service,
                [FromServices] IValidator<Hardware> validator,
                CancellationToken cancellationToken
        )
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var validationResult = await validator.ValidateAsync(hardware);
            if (!validationResult.IsValid)
            {
                return BadRequest();
            }

            var updated = await service.UpdateAsync(id, hardware, cancellationToken);

            if (!updated)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("hardwares/{id}")]
        public async Task<IActionResult> PutAsync(
                [FromRoute] string id,
                [FromServices] IHardwareService service,
                CancellationToken cancellationToken
        )
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            await service.DeleteAsync(id, cancellationToken);

            return NoContent();
        }

        [HttpPost("hardware")]
        public async Task<IActionResult> PostAsync(
                [FromBody] Hardware hardware,
                [FromServices] IHardwareService service,
                [FromServices] IValidator<Hardware> validator,
                CancellationToken cancellationToken
        )
        {
            var validationResult = await validator.ValidateAsync(hardware);
            if (!validationResult.IsValid)
            {
                return BadRequest();
            }

            var newHardware = await service.InsertAsync(hardware, cancellationToken);

            return Created($"/hardware/{newHardware.Id}", newHardware);
        }

        [HttpPatch("floors/{floorId}/hardwares/{hardwareId}")]
        public async Task<IActionResult> ImmobilizeAsync(
                [FromRoute] string floorId,
                [FromRoute] string hardwareId,
                [FromServices] IHardwareService service,
                CancellationToken cancellationToken
        )
        {
            if (string.IsNullOrWhiteSpace(floorId) ||
                String.IsNullOrWhiteSpace(hardwareId))
            {
                return BadRequest("Invalid arguments");
            }

            var updated = await service.ImmobilizeAsync(floorId, hardwareId, cancellationToken);

            return updated
                ? NoContent()
                : NotFound("The arguments were not found");
        }
    }
}
