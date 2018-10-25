using System.Collections.Generic;
using System.Threading.Tasks;
using MicroservicesChassis.ServiceDiscovery;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesChassis.Playground.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsulRegistryController : ControllerBase
    {
        private readonly IConsulServicesRegistry _registry;

        public ConsulRegistryController(IConsulServicesRegistry registry)
        {
            _registry = registry;
        }

        // GET api/values
        [HttpGet]
        [Route("{serviceName}")]
        public async Task<IActionResult> Get(string serviceName)
        {
            var service = await _registry.GetAsync(serviceName);
            if (service == null)
                return NotFound();

            return Ok(string.Format("{0}:{1}", service.Address, service.Port));
        }

    }
}