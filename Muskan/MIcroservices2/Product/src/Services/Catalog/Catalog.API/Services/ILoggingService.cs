namespace Catalog.API.Services
{
     public interface ILoggingService<T>
     {
          Task LogInformationAsync(string message);
          Task LogErrorAsync(string message, Exception exception);
          Task LogWarningAsync(string message);
          Task LogDebugAsync(string message);
          Task LogCriticalAsync(string message, Exception exception);
     }
}
