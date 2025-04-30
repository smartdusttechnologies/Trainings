namespace Basket.API.Service
{
     public static class LogEntryFactory
     {
          public static LogEntryMessage Create(string level, string message, string controllerName, IHttpContextAccessor httpContextAccessor, Exception? exception = null)
          {
               var context = httpContextAccessor.HttpContext;

               return new LogEntryMessage
               {
                    Timestamp = DateTime.UtcNow,
                    Level = level,
                    Message = exception != null ? $"{message} - {exception.Message}" : message,
                    ServiceName = "Basket",
                    Exception = exception?.ToString() ?? "No exception thrown",
                    ControllerName = controllerName,
                    CorrelationId = Guid.NewGuid().ToString(),
                    MachineName = Environment.MachineName,
                    HttpMethod = context?.Request?.Method ?? "N/A",
                    RequestPath = context?.Request?.Path ?? "N/A",
                    UserId = context?.User?.Identity?.Name ?? "anonymous",
                    SourceIP = context?.Connection?.RemoteIpAddress?.ToString() ?? "unknown"
               };
          }
     }
}
