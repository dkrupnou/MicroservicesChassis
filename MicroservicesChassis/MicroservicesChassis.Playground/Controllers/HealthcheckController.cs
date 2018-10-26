using Microsoft.AspNetCore.Mvc;

namespace MicroservicesChassis.Playground.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealthcheckController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}