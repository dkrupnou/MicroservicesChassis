﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroservicesChassis.ServiceDiscovery;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesChassis.Playground.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IConsulServicesRegistry _registry;

        public ValuesController(IConsulServicesRegistry registry)
        {
            _registry = registry;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var playground = await _registry.GetAsync("playgroud");
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}