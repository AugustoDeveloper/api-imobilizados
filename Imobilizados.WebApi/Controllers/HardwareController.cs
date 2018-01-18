using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Imobilizados.Application.Interfaces;
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
        public async Task<IActionResult> LoadAllAsync()
        {
            var collection = await _service.LoadAllAsync();
            return Ok(collection);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
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
