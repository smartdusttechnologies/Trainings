using BuildingBlock.Messaging.Events;
using Logging.API.Models;
using Microsoft.EntityFrameworkCore;
using NLog;
namespace Logging.API.Data
{

     public interface ILogEntryServices
     {
          Task SaveLogs(LogEntryMessage message);
     }
     public class LogEntryServices : ILogEntryServices
     {
          private readonly ApplicationDbContext _context;
          private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

          public LogEntryServices(ApplicationDbContext context)
          {
               _context = context;
          }

          public async Task SaveLogs(LogEntryMessage message)
          {
               Logger.Info("Getting the message from Kafka  and Store into db :", message);

               try
               {
                    var log = new LogEntry
                    {
                         Timestamp = message.Timestamp,
                         Level = message.Level,
                         Message = message.Message,
                         Exception = message.Exception,
                         MachineName = message.MachineName,
                         ServiceName = message.ServiceName,
                         ControllerName = message.ControllerName,
                         CorrelationId = message.CorrelationId,
                         RequestPath = message.RequestPath,
                         HttpMethod = message.HttpMethod,
                         UserId = message.UserId,
                         SourceIP = message.SourceIP
                    };
                    //if(log != null)
                    //{
                    //     Logger.Info("Messaage :" log.Message);
                    //     Logger.Info("Service :" log.ServiceName);
                    //     Logger.Info("TimeStamp :" log.Timestamp);
                    //     Logger.Info("---------------------------------------------------");

                    //}
                    Logger.Info("Getting Log and Log entry  :", log);
                    // Log the message based on its log level
                    LogMessage(log, message);

                    // Save the log entry to the database
                    _context.LogEntries.Add(log);
                    await _context.SaveChangesAsync();
               }
               catch (DbUpdateException dbEx)
               {
                    Logger.Error(dbEx, "Database update exception while saving log entry. Possible DB unavailability or schema mismatch.");
                    LogInnerExceptions(dbEx);
               }
               catch (InvalidOperationException invalidOpEx)
               {
                    Logger.Error(invalidOpEx, "Invalid operation while saving log entry.");
                    LogInnerExceptions(invalidOpEx);
               }
               catch (Exception ex)
               {
                    Logger.Error(ex, "Unexpected error while saving log entry.");
                    LogInnerExceptions(ex);
               }
          }

          private void LogMessage(LogEntry log, LogEntryMessage message)
          {
               switch (log.Level?.ToLower())
               {
                    case "information":
                         Logger.Info("-------------------------------Information----------------------------------");
                         Logger.Info($"Source: {message.ServiceName} - {message.ControllerName} Message: {log.Message}");
                         break;
                    case "warning":
                         Logger.Info("-------------------------------Warning----------------------------------");
                         Logger.Warn($"Source: {message.ServiceName} - {message.ControllerName} Message: {log.Message}");
                         break;
                    case "error":
                         Logger.Info("-------------------------------Error----------------------------------");
                         Logger.Error($"Source: {message.ServiceName} - {message.ControllerName} Message: {log.Message}");
                         break;
                    case "critical":
                         Logger.Info("-------------------------------Critical----------------------------------");
                         Logger.Fatal($"Source: {message.ServiceName} - {message.ControllerName} Message: {log.Message}");
                         break;
                    case "debug":
                         Logger.Info("-------------------------------Debug----------------------------------");
                         Logger.Debug($"Source: {message.ServiceName} - {message.ControllerName} Message: {log.Message}");
                         break;
                    case "trace":
                         Logger.Info("-------------------------------Trace----------------------------------");
                         Logger.Trace($"Source: {message.ServiceName} - {message.ControllerName} Message: {log.Message}");
                         break;
                    default:
                         Logger.Info("-------------------------------Other----------------------------------");
                         Logger.Info($"Source: {message.ServiceName} - {message.ControllerName} Message: {log.Message}");
                         break;
               }
          }
          private void LogInnerExceptions(Exception ex)
          {
               var current = ex.InnerException;
               while (current != null)
               {
                    Logger.Error(current, "Inner exception: {Message}", current.Message);
                    current = current.InnerException;
               }
          }
     }

}