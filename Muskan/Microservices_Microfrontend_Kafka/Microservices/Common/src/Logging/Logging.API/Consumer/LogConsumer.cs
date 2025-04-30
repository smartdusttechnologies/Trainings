//using BuildingBlock.Messaging.Consumer;
//using BuildingBlock.Messaging.Events;
//using Logging.API.Data;
//using Logging.API.Services;
//using RabbitMQ.Client;

//namespace Logging.API.Consumer
//{
//     /// <summary> 
//     /// Consumer class for processing log entry messages from a RabbitMQ queue.
//     /// </summary>
//     public class LogEntryConsumer : ConsumerServices
//     {
//          private readonly ILogger<LogEntryConsumer> _logger;
//          private readonly IServiceScopeFactory _serviceScopeFactory;

//          public LogEntryConsumer(
//              IConnectionFactory connectionFactory,
//              ILogger<LogEntryConsumer> logger,
//              IServiceScopeFactory serviceScopeFactory
//          ) : base(connectionFactory, logger) // Pass logger to the base class constructor
//          {
//               _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//               _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
//          }

//          protected override async Task HandleMessageAsync(LogEntryMessage message, CancellationToken cancellationToken)
//          {
//               // Create a scope to resolve the scoped services
//               using (var scope = _serviceScopeFactory.CreateScope())
//               {
//                    var serviceProvider = scope.ServiceProvider;  // Get IServiceProvider from the scope
//                    var logEntryService = serviceProvider.GetRequiredService<ILogEntryServices>();
//                    var logger = serviceProvider.GetRequiredService<ILogger<RabbitMqConsumerBackgroundService>>();

//                    // Now you can call SaveLogs on the scoped service
//                    await logEntryService.SaveLogs(message);
//               }
//          }
//     }
//}

//// private void LogMessage(Models.LogEntry log, LogEntryMessage message)
//// {
////      switch (log.Level?.ToLower())
////      {
////           case "information":
////                Logger.Info($"Source: {message.ServiceName} - {message.ControllerName} Message: {log.Message}");
////                break;
////           case "warning":
////                Logger.Warn($"Source: {message.ServiceName} - {message.ControllerName} Message: {log.Message}");
////                break;
////           case "error":
////                Logger.Error($"Source: {message.ServiceName} - {message.ControllerName} Message: {log.Message}");
////                break;
////           case "critical":
////                Logger.Fatal($"Source: {message.ServiceName} - {message.ControllerName} Message: {log.Message}");
////                break;
////           case "debug":
////                Logger.Debug($"Source: {message.ServiceName} - {message.ControllerName} Message: {log.Message}");
////                break;
////           case "trace":
////                Logger.Trace($"Source: {message.ServiceName} - {message.ControllerName} Message: {log.Message}");
////                break;
////           default:
////                Logger.Info($"Source: {message.ServiceName} - {message.ControllerName} Message: {log.Message}");
////                break;
////      }
//// }

