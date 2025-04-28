namespace Logging.API.Models
{
     public class LogEntry
     {
          /// <summary>
          /// Uniwur Identifier for the log entry.
          /// </summary>
          public int Id { get; set; }
          /// <summary>
          /// Timestamp of the log event in UTC.
          /// </summary>
          public DateTime Timestamp { get; set; }
          /// <summary>
          /// Log level (e.g., Information, Error).
          /// </summary>
          public string Level { get; set; }
          /// <summary>
          /// Message describing the log event. (main message)
          /// </summary>
          public string Message { get; set; }
          /// <summary>
          /// Exception message if an error occurred.
          /// </summary>
          public string? Exception { get; set; }
          /// <summary>
          /// Name of the machine  , hostname  or conatiner name  where the log was generated.
          /// </summary>
          public string MachineName { get; set; }
          /// <summary>
          /// Name of the Microdservice  generating the log.
          /// </summary>
          public string ServiceName { get; set; }
          /// <summary>
          /// Name of the controller and logic module name handling the request.
          /// </summary>
          public string ControllerName { get; set; }
          /// <summary>
          /// Unique identifier for the request, used for tracking and correlation.
          /// </summary>
          public string CorrelationId { get; set; }
          /// <summary>
          ///   HTTP route (e.g., /api/users)
          /// </summary>
          public string RequestPath { get; set; }
          /// <summary>
          /// HTTP method (e.g., GET, POST, PUT, DELETE).
          /// </summary>
          public string HttpMethod { get; set; }
          /// <summary>
          /// User identifier associated with the request (if applicable).
          /// </summary>
          public string UserId { get; set; }
          /// <summary>
          /// User agent string from the request (e.g., browser or client information).
          /// </summary>
          public string SourceIP { get; set; }
     }
}
