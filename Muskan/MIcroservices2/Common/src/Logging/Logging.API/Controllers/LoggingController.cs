using Logging.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Logging.API.Controllers
{
     [ApiController]
     [Route("api/[controller]")]
     public class LoggingController(ILogger<LoggingController> logger) : ControllerBase
     {
          [HttpPost]
          public async Task<IActionResult> ReceiveLog([FromBody] LogEntry logEntry)
          {
               // Set NLog context variables
               NLog.MappedDiagnosticsLogicalContext.Set("MachineName", logEntry.MachineName);
               NLog.MappedDiagnosticsLogicalContext.Set("ServiceName", logEntry.ServiceName);
               NLog.MappedDiagnosticsLogicalContext.Set("ControllerName", logEntry.ControllerName);
               NLog.MappedDiagnosticsLogicalContext.Set("CorrelationId", logEntry.CorrelationId);
               NLog.MappedDiagnosticsLogicalContext.Set("RequestPath", logEntry.RequestPath);
               NLog.MappedDiagnosticsLogicalContext.Set("HttpMethod", logEntry.HttpMethod);
               NLog.MappedDiagnosticsLogicalContext.Set("UserId", logEntry.UserId);
               NLog.MappedDiagnosticsLogicalContext.Set("SourceIP", logEntry.SourceIP);



               // Log based on level
               switch (logEntry.Level?.ToLower())
               {
                    case "information":
                         logger.LogInformation(logEntry.Message);
                         break;
                    case "warning":
                         logger.LogWarning(logEntry.Message);
                         break;
                    case "error":
                         logger.LogError(logEntry.Message);
                         break;
                    case "critical":
                         logger.LogCritical(logEntry.Message);
                         break;
                    case "debug":
                         logger.LogDebug(logEntry.Message);
                         break;
                    case "trace":
                         logger.LogTrace(logEntry.Message);
                         break;
                    default:
                         logger.LogInformation(logEntry.Message);
                         break;
               }

               return Ok(new { Success = true });
          }
     }
}
