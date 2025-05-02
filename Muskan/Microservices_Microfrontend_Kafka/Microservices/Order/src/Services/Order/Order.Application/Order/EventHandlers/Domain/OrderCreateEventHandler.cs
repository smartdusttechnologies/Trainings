//using MassTransit;
//using Microsoft.FeatureManagement;

//namespace Ordering.Application.Order.EventHandlers.Domain
//{
//     public class OrderCreateEventHandler(IPublishEndpoint publishEndpoint, IFeatureManager featureManager, ILoggingService<OrderCreateEventHandler> logger) : INotificationHandler<OrderCreatedEvent>
//     {
//          public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
//          {
//               await logger.LogInformationAsync($"Domain Event Handled : {domainEvent.GetType().Name}");
//               if (await featureManager.IsEnabledAsync("OrderFullFillment"))
//               {
//                    var orderCreatedIntegraionEvent = domainEvent.order.ToOrderDTO(); ;
//                    await publishEndpoint.Publish(orderCreatedIntegraionEvent, cancellationToken);
//               }
//          }
//     }
//}
using System.Text.Json;
using Microsoft.FeatureManagement;

namespace Ordering.Application.Order.EventHandlers.Domain
{
     public class OrderCreateEventHandler : INotificationHandler<OrderCreatedEvent>
     {
          private readonly IFeatureManager _featureManager;
          private readonly ILoggingService<OrderCreateEventHandler> _logger;
          private const string TopicName = "order_created_topic";
          private readonly IProducerServices _producerService;

          public OrderCreateEventHandler(IFeatureManager featureManager, ILoggingService<OrderCreateEventHandler> logger, IProducerServices producerService)
          {
               _featureManager = featureManager;
               _logger = logger;             
               _producerService = producerService;
          }

          public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
          {
               await _logger.LogInformationAsync($"Domain Event Handled: {domainEvent.GetType().Name}");

               if (await _featureManager.IsEnabledAsync("OrderFullFillment"))
               {
                    // Convert domain event to the DTO that you will publish
                    var orderCreatedIntegrationEvent = domainEvent.order.ToOrderDTO();

                    // Publish the event to Kafka
                    var jsonMessage = JsonSerializer.Serialize(orderCreatedIntegrationEvent);
                    await _producerService.ProduceAsync(TopicName, jsonMessage);
                    await _logger.LogInformationAsync("Order creation event published to Kafka.");
               }
          }

          //private async Task PublishToRabbitMQAsync(object message, CancellationToken cancellationToken)
          //{
          //     try
          //     {
          //          using (var connection = await _connectionFactory.CreateConnectionAsync())
          //          using (var channel = await connection.CreateChannelAsync())
          //          {
          //               // Declare exchange
          //               await channel.ExchangeDeclareAsync(_exchange, ExchangeType.Fanout, durable: true, cancellationToken: cancellationToken);

          //               // Serialize the message to JSON
          //               var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

          //               // Create basic properties (required for type inference)
          //               var basicProperties = new BasicProperties
          //               {
          //                    ContentType = "application/json"
          //               };

          //               // Publish the message to the exchange
          //               await channel.BasicPublishAsync(
          //                   exchange: _exchange,
          //                   routingKey: _routingKey,
          //                   mandatory: false,
          //                   basicProperties: basicProperties,
          //                   body: messageBody,
          //                   cancellationToken: cancellationToken);

          //               await _logger.LogInformationAsync("Message published to RabbitMQ.");
          //          }
          //     }
          //     catch (Exception ex)
          //     {
          //          await _logger.LogErrorAsync($"Error publishing message to RabbitMQ: {ex.Message}", ex);
          //     }
          //}
     }
}
