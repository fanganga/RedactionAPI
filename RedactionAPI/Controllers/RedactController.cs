using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using RedactionAPI.Services;

namespace RedactionAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedactController : ControllerBase
    {
    
        private readonly ILogger<RedactController> _logger;
        private readonly IRedactService _redactService;

        public RedactController(ILogger<RedactController> logger, IRedactService redact)
        {
            _redactService = redact;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("Redaction Service");
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post()
        {
            using StreamReader reader = new StreamReader(Request.Body, leaveOpen: false);
            string message = await reader.ReadToEndAsync();
            return Ok(_redactService.Redact(message));
        }
    }
}
