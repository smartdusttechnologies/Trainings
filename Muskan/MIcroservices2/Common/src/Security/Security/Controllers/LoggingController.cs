using Microsoft.AspNetCore.Mvc;

namespace Security.Controllers
{
     [ApiController]
     [Route("api/logs")]
     public class LoggingController : ControllerBase
     {
          private readonly ILogger<LoggingController> _logger;

          public LoggingController(ILogger<LoggingController> logger)
          {
               _logger = logger;
          }

          [HttpPost]
          public IActionResult LogMessage([FromBody] LogRequest logRequest)
          {
               if (logRequest == null || string.IsNullOrEmpty(logRequest.Message))
               {
                    return BadRequest("Invalid log request.");
               }

               switch (logRequest.LogLevel)
               {
                    case "Information":
                         _logger.LogInformation("{Source} - {Message}", logRequest.Source, logRequest.Message);
                         break;
                    case "Warning":
                         _logger.LogWarning("{Source} - {Message}", logRequest.Source, logRequest.Message);
                         break;
                    case "Error":
                         _logger.LogError("{Source} - {Message}", logRequest.Source, logRequest.Message);
                         break;
                    default:
                         _logger.LogDebug("{Source} - {Message}", logRequest.Source, logRequest.Message);
                         break;
               }

               return Ok(new { Message = "Log stored successfully" });
          }
     }
     public class LogRequest
     {
          public string LogLevel { get; set; }  // Information, Warning, Error
          public string Message { get; set; }
          public string Source { get; set; }  // Microservice Name
     }
}
