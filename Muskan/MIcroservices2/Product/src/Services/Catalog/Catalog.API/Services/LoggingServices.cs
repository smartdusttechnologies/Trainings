using System.Text;
using Newtonsoft.Json;

namespace Catalog.API.Services
{

     public class LoggingServices : ILoggingService
     {
          private readonly HttpClient _httpClient;
          private readonly IConfiguration _configuration;
          private readonly string _loggingEndpoint;

          public LoggingServices(HttpClient httpClient, IConfiguration configuration)
          {
               _httpClient = httpClient;
               _configuration = configuration;
               _loggingEndpoint = _configuration["CommonService:Logging"];
          }

          public async Task LogInformationAsync(string message, string controllerName)
          {
               var logEntry = new LogEntry
               {
                    Timestamp = DateTime.UtcNow,
                    Level = "Information",
                    Message = message,
                    ServiceName = "Products",
                    ControllerName = controllerName,
                    CorrelationId = Guid.NewGuid().ToString()

               };
               try
               {
                    await SendLogAsync(logEntry);
               }
               catch (Exception ex)
               {

                    Console.WriteLine($"Logging failed: {ex.Message}");
               }


          }

          public async Task LogErrorAsync(string message, string controllerName, Exception exception)
          {
               var logEntry = new LogEntry
               {
                    Timestamp = DateTime.UtcNow,
                    Level = "Error",
                    Message = $"{message}: {exception.Message}",
                    ServiceName = "Products",
                    ControllerName = controllerName,
                    CorrelationId = Guid.NewGuid().ToString(),

               };

               try
               {
                    await SendLogAsync(logEntry);
               }
               catch (Exception ex)
               {

                    Console.WriteLine($"Logging failed: {ex.Message}");
               }
          }

          private async Task SendLogAsync(LogEntry logEntry)
          {
               var json = JsonConvert.SerializeObject(logEntry);
               var content = new StringContent(json, Encoding.UTF8, "application/json");
               await _httpClient.PostAsync(_loggingEndpoint, content);
          }
     }
}
