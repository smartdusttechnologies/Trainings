using BuildingBlock.Messaging.Events;
using BuildingBlock.Messaging.Producer;
using Microsoft.AspNetCore.Http;
using RabbitMQ.Client;

namespace Ordering.Application.Services
{
     public class LoggingService<T> : RabbitMqProducerBase<LogEntryMessage>, ILoggingService<T> where T : class
     {
          private readonly IHttpContextAccessor _httpContextAccessor;
          private readonly string _controllerName;

          // Add IConnectionFactory and Exchange name to constructor
          public LoggingService(IHttpContextAccessor httpContextAccessor, IConnectionFactory connectionFactory)
           : base(connectionFactory, "log-entry-queue", "log-entry-queue")
          {
               _httpContextAccessor = httpContextAccessor;
               _controllerName = typeof(T).Name;
          }

          private async Task PublishLogAsync(string level, string message, Exception? exception = null)
          {
               var context = _httpContextAccessor.HttpContext;
               var requestPath = context?.Request?.Path.ToString();
               if (!string.IsNullOrEmpty(requestPath) && !requestPath.StartsWith("/"))
               {
                    requestPath = "/" + requestPath;
               }
               var logEntry = new LogEntryMessage
               {
                    Timestamp = DateTime.UtcNow,
                    Level = level,
                    Message = exception != null ? $"{message} - {exception.Message}" : message,
                    ServiceName = "Order",
                    Exception = exception?.ToString() ?? "No exception thrown",
                    ControllerName = _controllerName,
                    CorrelationId = Guid.NewGuid().ToString(),
                    MachineName = Environment.MachineName,
                    HttpMethod = context?.Request?.Method ?? "N/A",
                    RequestPath = requestPath ?? "N/A",
                    UserId = context?.User?.Identity?.Name ?? "anonymous",
                    SourceIP = context?.Connection?.RemoteIpAddress?.ToString() ?? "unknown"
               };

               await Publish(logEntry);
               await Task.CompletedTask;
          }

          public Task LogInformationAsync(string message) => PublishLogAsync("Information", message);
          public Task LogErrorAsync(string message, Exception exception) => PublishLogAsync("Error", message, exception);
          public Task LogWarningAsync(string message) => PublishLogAsync("Warning", message);
          public Task LogDebugAsync(string message) => PublishLogAsync("Debug", message);
          public Task LogCriticalAsync(string message, Exception exception) => PublishLogAsync("Critical", message, exception);
     }
}
