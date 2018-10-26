using System.Collections.Generic;
using System.Threading.Tasks;
using MicroservicesChassis.ApiGateway.Clients.Playground;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesChassis.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaygroundController : ControllerBase
    {
        private readonly IPlaygorundClient _playgroundClient;

        public PlaygroundController(IPlaygorundClient playgroundClient)
        {
            _playgroundClient = playgroundClient;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            return await _playgroundClient.GetAsync();
        }
    }
}
