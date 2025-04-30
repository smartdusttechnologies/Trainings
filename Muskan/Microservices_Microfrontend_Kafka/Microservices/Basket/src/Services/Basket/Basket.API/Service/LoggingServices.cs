using System.Text.Json;

namespace Basket.API.Service
{
     public class LoggingService<T> : ILoggingService<T> where T : class
     {
          private readonly IHttpContextAccessor _httpContextAccessor;
          private readonly IProducerServices _producerService;
          private readonly ILogger<LoggingService<T>> _logger;
          private readonly string _controllerName;
          private const string TopicName = "log-entry-topic";

          public LoggingService(
              IHttpContextAccessor httpContextAccessor,
              IProducerServices producerService,
              ILogger<LoggingService<T>> logger)
          {
               _httpContextAccessor = httpContextAccessor;
               _producerService = producerService;
               _logger = logger;
               _controllerName = typeof(T).Name;
          }

          private async Task PublishLogAsync(string level, string message, Exception? exception = null)
          {
               var logEntry = LogEntryFactory.Create(level, message, _controllerName, _httpContextAccessor, exception);
               var jsonMessage = JsonSerializer.Serialize(logEntry);

               await _producerService.ProduceAsync(TopicName, jsonMessage);
          }
          public Task LogInformationAsync(string message) => PublishLogAsync("Information", message);
          public Task LogErrorAsync(string message, Exception exception) => PublishLogAsync("Error", message, exception);
          public Task LogWarningAsync(string message) => PublishLogAsync("Warning", message);
          public Task LogDebugAsync(string message) => PublishLogAsync("Debug", message);
          public Task LogCriticalAsync(string message, Exception exception) => PublishLogAsync("Critical", message, exception);
     }
}
