using Logging.API.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace Logging.API.Controllers
{
     [ApiController]
     [Route("api/[controller]")]
     public class LoggingController(ILogger<LoggingController> logger, IElasticClient elasticClient) : ControllerBase
     {


          [HttpPost]
          public async Task<IActionResult> ReceiveLog([FromBody] LogEntry logEntry)
          {
               // Send to Elasticsearch
               var indexResponse = await elasticClient.IndexDocumentAsync(logEntry);
               if (logEntry.Level == "Information")
               {

                    logger.LogInformation(logEntry.Message);
               }
               else if (logEntry.Level == "Warning")
               {

                    logger.LogWarning(logEntry.Message);
               }
               else if (logEntry.Level == "Error")
               {

                    logger.LogError(logEntry.Message);
               }
               else if (logEntry.Level == "Critical")
               {

                    logger.LogCritical(logEntry.Message);
               }
               else if (logEntry.Level == "Debug")
               {

                    logger.LogDebug(logEntry.Message);
               }
               else if (logEntry.Level == "Trace")
               {

                    logger.LogTrace(logEntry.Message);
               }
               else
               {

                    logger.LogInformation(logEntry.Message);
               }

               return Ok(new
               {
                    Success = true,
                    DocumentId = indexResponse.Id
               });
          }
     }
}
