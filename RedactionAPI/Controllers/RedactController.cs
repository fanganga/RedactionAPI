using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using RedactionAPI.Services;
using RedactionAPI.Utilities.Logging;

namespace RedactionAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedactController(ILogger<RedactController> logger,
            IRedactService redactService,
            IInformationService informationService,
            ICustomLogger customLogger) : ControllerBase
    {
    
        private readonly ILogger<RedactController> _logger = logger;
        private readonly IRedactService _redactService = redactService;
        private readonly IInformationService _informationService = informationService;
        private readonly ICustomLogger _customLogger = customLogger;

        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok(_informationService.GetInformation());
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post()
        {
            try
            {
                using StreamReader reader = new(Request.Body, leaveOpen: false);
                string message = await reader.ReadToEndAsync();
                string timestamp = DateTime.UtcNow.ToString();
                _logger.LogInformation("{Timestamp}: React Post with message: {Message}",
                    timestamp, message);
                _customLogger.WriteLogLine($"{timestamp} React Post with message: {message}");
                return Ok(_redactService.Redact(message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem();
            }
        }
    }
}
