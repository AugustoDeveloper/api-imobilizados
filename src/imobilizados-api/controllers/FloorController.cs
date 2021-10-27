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
    public class FloorController : ControllerBase
    {
        [HttpGet("floors/{id}")]
        public async Task<IActionResult> GetASync(
                [FromRoute] string id,
                [FromServices] IFloorService service,
                CancellationToken cancellationToken
        )
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var floor = await service.GetByIdAsync(id, cancellationToken);
            if (floor == null)
            {
                return NotFound();
            }

            return Ok(floor);
        }

        [HttpGet("floors")]
        public async Task<IActionResult> GetAllAsync(
                [FromServices] IFloorService service,
                CancellationToken cancellationToken
        )
        {
            var floors = await service.GetAllAsync(cancellationToken);
            return Ok(floors);
        }

        [HttpPut("floors/{id}")]
        public async Task<IActionResult> PutAsync(
                [FromRoute] string id,
                [FromBody] Floor floor,
                [FromServices] IFloorService service,
                [FromServices] IValidator<Floor> validator,
                CancellationToken cancellationToken
        )
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var validationResult = await validator.ValidateAsync(floor);
            if (!validationResult.IsValid)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("floors/{id}")]
        public async Task<IActionResult> DeleteAsync(
                [FromRoute] string id,
                [FromServices]IFloorService service
        )
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            await service.DeleteAsync(id);

            return NoContent();
        }

        [HttpPost("floor")]
        public async Task<IActionResult> PostAsync(
                [FromBody] Floor floor,
                [FromServices] IFloorService service,
                [FromServices] IValidator<Floor> validator,
                CancellationToken cancellationToken
        )
        {
            var validationResult = await validator.ValidateAsync(floor);
            if (!validationResult.IsValid)
            {
                return BadRequest();
            }

            var newFloor = await service.InsertAsync(floor, cancellationToken);

            return Created($"/floor/{newFloor.Id}", newFloor);
        }
    }
}

