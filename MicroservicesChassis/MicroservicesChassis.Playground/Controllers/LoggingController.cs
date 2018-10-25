using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MicroservicesChassis.Playground.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggingController : ControllerBase
    {
        private readonly ILogger _logger;

        public LoggingController(ILogger<LoggingController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var datetime = DateTime.Now;
            _logger.LogWarning(datetime.ToString());
            return Ok(string.Format("Current date time was logged: {0}", datetime));
        }
    }
}