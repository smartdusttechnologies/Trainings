
using BuildingBlock.Messaging.Events;
using Logging.API.Data;
using MassTransit;

namespace Logging.API.Consumer
{
     public class LogEntryConsumer : IConsumer<LogEntryMessage>
     {
          private readonly ApplicationDbContext _context;

          public LogEntryConsumer(ApplicationDbContext context)
          {
               _context = context;
          }

          public async Task Consume(ConsumeContext<LogEntryMessage> context)
          {
               var message = context.Message;
               //Console.WriteLine($"[LOG RECEIVED] {message.Level} - {message.Message}");
               var log = new Models.LogEntry
               {
                    Timestamp = context.Message.Timestamp,
                    Level = context.Message.Level,
                    Message = context.Message.Message,
                    Exception = context.Message.Exception,
                    MachineName = context.Message.MachineName,
                    ServiceName = context.Message.ServiceName,
                    ControllerName = context.Message.ControllerName,
                    CorrelationId = context.Message.CorrelationId,
                    RequestPath = context.Message.RequestPath,
                    HttpMethod = context.Message.HttpMethod,
                    UserId = context.Message.UserId,
                    SourceIP = context.Message.SourceIP
               };

               _context.LogEntries.Add(log);
               await _context.SaveChangesAsync();
          }
     }
}
