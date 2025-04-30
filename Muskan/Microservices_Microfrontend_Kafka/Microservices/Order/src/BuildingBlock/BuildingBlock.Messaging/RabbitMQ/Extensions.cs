using BuildingBlock.Messaging.Consumer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace BuildingBlock.Messaging.RabbitMQ
{
     public static class Extensions
     {
          /// <summary>
          /// Adds RabbitMQ client configuration to the service collection.
          /// </summary>
          /// <param name="services">The service collection to add dependencies.</param>
          /// <param name="configuration">Application configuration settings.</param>
          /// <returns>The updated service collection.</returns>
          public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
          {
               // Register RabbitMQ connection factory
               services.AddSingleton<IConnectionFactory>(sp =>
               {
                    var host = configuration["MessageBroker:Host"];
                    var username = configuration["MessageBroker:Username"];
                    var password = configuration["MessageBroker:Password"];

                    return new ConnectionFactory
                    {
                         Uri = new Uri(host),
                         UserName = username,
                         ClientProvidedName = "Order.API",
                         Password = password,
                    };
               });
               services.AddHostedService<RabbitMqConsumerBackgroundService>();
               return services;
          }
     }
}