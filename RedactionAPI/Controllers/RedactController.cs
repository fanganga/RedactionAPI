using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using RedactionAPI.Services;
using RedactionAPI.Utilities.Logging;

namespace RedactionAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedactController : ControllerBase
    {
    
        private readonly ILogger<RedactController> _logger;
        private readonly IRedactService _redactService;
        private readonly ICustomLogger _customLogger;

        public RedactController(ILogger<RedactController> logger, IRedactService redact, ICustomLogger customLogger)
        {
            _redactService = redact;
            _logger = logger;
            _customLogger = customLogger;
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
            _logger.LogInformation("{Timestamp}: React Post with message: {Message}", 
                DateTime.UtcNow.ToString(), message);
            _customLogger.WriteLogLine($"{DateTime.UtcNow.ToString()} React Post with message: {message}");
            return Ok(_redactService.Redact(message));
        }
    }
}
