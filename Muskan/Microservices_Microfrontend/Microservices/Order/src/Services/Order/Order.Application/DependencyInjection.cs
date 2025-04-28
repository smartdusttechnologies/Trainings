using System.Reflection;
using BuildingBlock.Behaviour;
using BuildingBlock.Messaging.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Ordering.Application.Middleware;
namespace Ordering.Application
{
     public static class DependencyInjection
     {
          public static IServiceCollection AddApplicationService(this IServiceCollection service, IConfiguration configuration)
          {

               service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


               // MediatR behaviors
               service.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
               service.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

               // Ensure FluentValidation validators are registered
               service.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
               // Register HTTP clients
               service.AddHttpClient();
               service.AddHttpClient<TokenValidator>();

               // Register logging service
               service.AddScoped(typeof(ILoggingService<>), typeof(LoggingService<>));
               //service.AddMediatR(cfg =>
               //{
               //    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
               //    cfg.AddBehavior(typeof(ValidationBehaviour<,>));
               //    cfg.AddBehavior(typeof(LoggingBehaviour<,>));
               //});
               //service.AddHttpClient<TokenValidator>();
               //        service.AddHttpClient<TokenValidator>()
               //.ConfigurePrimaryHttpMessageHandler(() =>
               //{
               //     var handler = new HttpClientHandler
               //     {
               //          ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
               //     };


               // Fetaure  Management 
               service.AddFeatureManagement();
               // Add Meesage Broker 
               service.AddRabbitMq(configuration);
               return service;
          }
     }
}
