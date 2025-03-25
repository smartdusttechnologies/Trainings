using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlock.Messaging.MassTransit
{
    public static class Extensions
    {
        /// <summary>
        /// Adds a message broker using MassTransit and RabbitMQ.
        /// Configures message bus, consumers, and RabbitMQ host settings.
        /// </summary>
        /// <param name="services">The service collection to add dependencies.</param>
        /// <param name="configuration">Application configuration settings.</param>
        /// <param name="assembly">Optional assembly for scanning consumers.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
        {
            // Register MassTransit with RabbitMQ
            services.AddMassTransit(config =>
            {
                // Use kebab-case naming convention for endpoints
                config.SetKebabCaseEndpointNameFormatter();

                // Register consumers if an assembly is provided
                if (assembly != null)
                {
                    config.AddConsumers(assembly);
                }

                // Configure RabbitMQ as the transport
                config.UsingRabbitMq((context, cfg) =>
                {
                    // Retrieve RabbitMQ configuration from appsettings
                    var host = configuration["MessageBroker:Host"];
                    var username = configuration["MessageBroker:Username"];
                    var password = configuration["MessageBroker:Password"];                

                    // Set up the RabbitMQ host connection
                    cfg.Host(new Uri(host), h =>
                    {
                        h.Username(username);
                        h.Password(password);
                    });
                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
