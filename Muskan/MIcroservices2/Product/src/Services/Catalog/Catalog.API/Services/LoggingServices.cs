using BuildingBlock.Messaging.Events;
using MassTransit;

namespace Catalog.API.Services
{
     public class LoggingService<T> : ILoggingService<T>
     {
          private readonly IPublishEndpoint _publishEndpoint;
          private readonly IHttpContextAccessor _httpContextAccessor;
          private readonly string _controllerName;

          public LoggingService(IPublishEndpoint publishEndpoint, IHttpContextAccessor httpContextAccessor)
          {
               _publishEndpoint = publishEndpoint;
               _httpContextAccessor = httpContextAccessor;
               _controllerName = typeof(T).Name;
          }

          private async Task PublishLogAsync(string level, string message, Exception? exception = null)
          {
               var context = _httpContextAccessor.HttpContext;

               var logEntry = new LogEntryMessage
               {
                    Timestamp = DateTime.UtcNow,
                    Level = level,
                    Message = exception != null ? $"{message} - {exception.Message}" : message,
                    ServiceName = "Products",
                    ControllerName = _controllerName,
                    CorrelationId = Guid.NewGuid().ToString(),
                    MachineName = Environment.MachineName,
                    HttpMethod = context?.Request?.Method ?? "N/A",
                    RequestPath = context?.Request?.Path ?? "N/A",
                    UserId = context?.User?.Identity?.Name ?? "anonymous",
                    SourceIP = context?.Connection?.RemoteIpAddress?.ToString() ?? "unknown"
               };

               await _publishEndpoint.Publish(logEntry);
          }

          public Task LogInformationAsync(string message) => PublishLogAsync("Information", message);
          public Task LogErrorAsync(string message, Exception exception) => PublishLogAsync("Error", message, exception);
          public Task LogWarningAsync(string message) => PublishLogAsync("Warning", message);
          public Task LogDebugAsync(string message) => PublishLogAsync("Debug", message);
          public Task LogCriticalAsync(string message, Exception exception) => PublishLogAsync("Critical", message, exception);
     }
}
