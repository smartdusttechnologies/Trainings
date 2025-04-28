using BuildingBlock.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Ordering.Application.Middleware;
using Ordering.Application.Order.EventHandlers.Integration;

namespace Ordering.API
{
     public static class DependencyInjection
     {
          public static IServiceCollection AddAPIService(this IServiceCollection service, IConfiguration configuration)
          {
               var assembly = typeof(Program).Assembly;
               service.AddAutoMapper(assembly);
               // Register your custom background service
               //service.AddRabbitMq(configuration);
               service.AddHostedService<BasketCheckOutEventHandler>();
               service.AddControllers();
               service.AddExceptionHandler<CustomExceptionHandler>();
               service.AddHealthChecks()
                    .AddSqlServer(configuration.GetConnectionString("SqlServerDb"));


               return service;
          }
          public static WebApplication UseAPIService(this WebApplication app)
          {
               //app.MapCarter();
               app.UseExceptionHandler(options => { });
               app.UseMiddleware<TokenValidator>();
               app.UseRouting();
               app.MapControllers();
               app.UseHealthChecks("/healths",
                   new HealthCheckOptions
                   {
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                   });
               return app;
          }
     }
}
