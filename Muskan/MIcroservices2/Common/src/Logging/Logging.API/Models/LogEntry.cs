namespace Logging.API.Models
{
     public class LogEntry
     {
          public DateTime Timestamp { get; set; }
          public string Level { get; set; }
          public string Message { get; set; }
          public string ServiceName { get; set; }
          public string ControllerName { get; set; }

          public string CorrelationId { get; set; }
     }
}
