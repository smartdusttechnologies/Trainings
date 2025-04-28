using Logging.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Logging.API.Controllers
{
     [ApiController]
     [Route("[controller]")]
     public class LoggingController : ControllerBase
     {
          private readonly ApplicationDbContext _context;

          public LoggingController(ApplicationDbContext context)
          {
               _context = context;
          }


          /// <summary>
          /// GET: api/LogEntry
          /// Retrieves all log entries from the database.
          /// </summary>
          [HttpGet]
          public async Task<ActionResult<IEnumerable<Models.LogEntry>>> GetLogEntries()
          {
               // Retrieve log entries from the database.
               var logEntries = await _context.LogEntries.ToListAsync();

               if (logEntries == null || !logEntries.Any())
               {
                    return NotFound("No log entries found.");
               }

               return Ok(logEntries);
          }

          /// <summary>
          /// GET: api/LogEntry/{id}
          /// Retrieves a single log entry based on the ID.
          /// </summary>
          [HttpGet("{id}")]
          public async Task<ActionResult<Models.LogEntry>> GetLogEntry(int id)
          {
               // Retrieve the log entry by its ID.
               var logEntry = await _context.LogEntries.FindAsync(id);

               if (logEntry == null)
               {
                    return NotFound($"Log entry with ID {id} not found.");
               }

               return Ok(logEntry);
          }
     }
}
//[HttpPost]
//public async Task<IActionResult> ReceiveLog([FromBody] LogEntry logEntry)
//{
//     // Set NLog context variables
//     NLog.MappedDiagnosticsLogicalContext.Set("MachineName", logEntry.MachineName);
//     NLog.MappedDiagnosticsLogicalContext.Set("ServiceName", logEntry.ServiceName);
//     NLog.MappedDiagnosticsLogicalContext.Set("ControllerName", logEntry.ControllerName);
//     NLog.MappedDiagnosticsLogicalContext.Set("CorrelationId", logEntry.CorrelationId);
//     NLog.MappedDiagnosticsLogicalContext.Set("RequestPath", logEntry.RequestPath);
//     NLog.MappedDiagnosticsLogicalContext.Set("HttpMethod", logEntry.HttpMethod);
//     NLog.MappedDiagnosticsLogicalContext.Set("UserId", logEntry.UserId);
//     NLog.MappedDiagnosticsLogicalContext.Set("SourceIP", logEntry.SourceIP);



//     // Log based on level
//     switch (logEntry.Level?.ToLower())
//     {
//          case "information":
//               logger.LogInformation(logEntry.Message);
//               break;
//          case "warning":
//               logger.LogWarning(logEntry.Message);
//               break;
//          case "error":
//               logger.LogError(logEntry.Message);
//               break;
//          case "critical":
//               logger.LogCritical(logEntry.Message);
//               break;
//          case "debug":
//               logger.LogDebug(logEntry.Message);
//               break;
//          case "trace":
//               logger.LogTrace(logEntry.Message);
//               break;
//          default:
//               logger.LogInformation(logEntry.Message);
//               break;
//     }

//     return Ok(new { Success = true });
//}
