using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebReceiverDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost("receiver-form-content")]
        public IActionResult Post([FromForm] TestSubmit form)
        {
            return Ok(new { success = true, data = form });
        }

        [HttpGet("receiver-form-content")]
        public IActionResult Get([FromQuery] TestSubmit form)
        {
            return Ok(new { success = true, data = form });
        }
    }

    public class TestSubmit
    {
        public int Id { get; set; } = 1;

        public string Content { get; set; }
    }
}