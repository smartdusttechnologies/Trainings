using System.Reflection;
using BuildingBlock.Behaviour;
using BuildingBlock.Messaging.MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Ordering.Application.Middleware;
using Ordering.Application.Services;

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
               service.AddHttpClient();
               service.AddScoped(typeof(ILoggingService<>), typeof(LoggingService<>));
               //service.AddMediatR(cfg =>
               //{
               //    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
               //    cfg.AddBehavior(typeof(ValidationBehaviour<,>));
               //    cfg.AddBehavior(typeof(LoggingBehaviour<,>));
               //});
               service.AddHttpClient<TokenValidator>()
       .ConfigurePrimaryHttpMessageHandler(() =>
       {
            var handler = new HttpClientHandler
            {
                 ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            return handler;
       });

               // Fetaure  Management 
               service.AddFeatureManagement();
               // Add Meesage Broker 
               service.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());
               return service;
          }
     }
}
