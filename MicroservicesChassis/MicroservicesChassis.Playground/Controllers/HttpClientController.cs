using System.Net.Http;
using System.Threading.Tasks;
using MicroservicesChassis.ServiceDiscovery;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesChassis.Playground.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HttpClientController : ControllerBase
    {
        private IConsulHttpClient _client;

        public HttpClientController(IConsulHttpClient consulHttpClient)
        {
            _client = consulHttpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await _client.GetAsync<bool>("playground");
            return Ok();
        }
    }
}