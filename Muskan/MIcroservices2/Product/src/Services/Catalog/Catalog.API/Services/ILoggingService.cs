namespace Catalog.API.Services
{
     public interface ILoggingService
     {
          Task LogInformationAsync(string message, string controllerName);
          Task LogErrorAsync(string message, string controllerName, Exception exception);
     }
}
