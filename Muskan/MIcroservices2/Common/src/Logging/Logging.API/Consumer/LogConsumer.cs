
//using BuildingBlock.Messaging.Events;
//using Logging.API.Data;
//using MassTransit;

//namespace Logging.API.Consumer
//{
//     public class LogEntryConsumer : IConsumer<LogEntryMessage>
//     {
//          private readonly ApplicationDbContext _context;
//          private readonly ILogger<LogEntryConsumer> _logger;

//          public LogEntryConsumer(ApplicationDbContext context, ILogger<LogEntryConsumer> logger)
//          {
//               _context = context;
//               _logger = logger;
//          }

//          public async Task Consume(ConsumeContext<LogEntryMessage> context)
//          {
//               var message = context.Message;
//               //Console.WriteLine($"[LOG RECEIVED] {message.Level} - {message.Message}");
//               var log = new Models.LogEntry
//               {
//                    Timestamp = context.Message.Timestamp,
//                    Level = context.Message.Level,
//                    Message = context.Message.Message,
//                    Exception = context.Message.Exception,
//                    MachineName = context.Message.MachineName,
//                    ServiceName = context.Message.ServiceName,
//                    ControllerName = context.Message.ControllerName,
//                    CorrelationId = context.Message.CorrelationId,
//                    RequestPath = context.Message.RequestPath,
//                    HttpMethod = context.Message.HttpMethod,
//                    UserId = context.Message.UserId,
//                    SourceIP = context.Message.SourceIP
//               };
//               switch (log.Level?.ToLower())
//               {
//                    case "information":
//                         _logger.LogInformation($"Source : {context.Message.ServiceName}  - {context.Message.ControllerName} Messsage : {log.Message}");
//                         break;
//                    case "warning":
//                         _logger.LogWarning($"Source : {context.Message.ServiceName}  - {context.Message.ControllerName} Messsage : {log.Message}");
//                         break;
//                    case "error":
//                         _logger.LogError($"Source : {context.Message.ServiceName}  - {context.Message.ControllerName} Messsage : {log.Message}");
//                         break;
//                    case "critical":
//                         _logger.LogCritical($"Source : {context.Message.ServiceName}  - {context.Message.ControllerName} Messsage : {log.Message}");
//                         break;
//                    case "debug":
//                         _logger.LogDebug($"Source : {context.Message.ServiceName}  - {context.Message.ControllerName} Messsage : {log.Message}");
//                         break;
//                    case "trace":
//                         _logger.LogTrace($"Source : {context.Message.ServiceName}  - {context.Message.ControllerName} Messsage : {log.Message}");
//                         break;
//                    default:
//                         _logger.LogInformation($"Source : {context.Message.ServiceName}  - {context.Message.ControllerName} Messsage : {log.Message}");
//                         break;
//               }

//               _context.LogEntries.Add(log);
//               await _context.SaveChangesAsync();
//          }
//     }
//}
