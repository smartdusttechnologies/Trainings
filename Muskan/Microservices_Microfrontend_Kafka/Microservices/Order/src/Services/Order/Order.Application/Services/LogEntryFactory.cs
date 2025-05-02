using BuildingBlock.Messaging.Events;
using Microsoft.AspNetCore.Http;

namespace Ordering.Application.Services
{
     public static class LogEntryFactory
     {
          public static LogEntryMessage Create(string level, string message, string controllerName, IHttpContextAccessor httpContextAccessor, Exception? exception = null)
          {
               var context = httpContextAccessor.HttpContext;
               //var context = _httpContextAccessor.HttpContext;
               var requestPath = context?.Request?.Path.ToString();
               if (!string.IsNullOrEmpty(requestPath) && !requestPath.StartsWith("/"))
               {
                    requestPath = "/" + requestPath;
               }
               return new LogEntryMessage
               {
                    Timestamp = DateTime.UtcNow,
                    Level = level,
                    Message = exception != null ? $"{message} - {exception.Message}" : message,
                    ServiceName = "Order",
                    Exception = exception?.ToString() ?? "No exception thrown",
                    ControllerName = controllerName,
                    CorrelationId = Guid.NewGuid().ToString(),
                    MachineName = Environment.MachineName,
                    HttpMethod = context?.Request?.Method ?? "N/A",
                    RequestPath = requestPath ?? "N/A",
                    UserId = context?.User?.Identity?.Name ?? "anonymous",
                    SourceIP = context?.Connection?.RemoteIpAddress?.ToString() ?? "unknown"
               };
          }
     }
}
