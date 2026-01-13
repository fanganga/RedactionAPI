using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace RedactionAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedactController : ControllerBase
    {
    
        private readonly ILogger<RedactController> _logger;

        public RedactController(ILogger<RedactController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("Redaction Service");
        }
    }
}
