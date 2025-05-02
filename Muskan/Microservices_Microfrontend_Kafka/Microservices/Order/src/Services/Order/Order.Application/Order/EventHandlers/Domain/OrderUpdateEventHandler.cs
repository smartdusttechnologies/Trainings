//namespace Ordering.Application.Order.EventHandlers.Domain
//{
//     public class OrderUpdateEventHandler(ILoggingService<OrderUpdateEventHandler> logger) : INotificationHandler<OrderUpdatedEvent>
//     {
//          public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
//          {
//               logger.LogInformationAsync($"Domain Event Handled : {notification.GetType().Name}");
//               return Task.CompletedTask;
//          }
//     }
//}

using System.Text.Json;

namespace Ordering.Application.Order.EventHandlers.Domain
{
     public class OrderUpdateEventHandler : INotificationHandler<OrderUpdatedEvent>
     {
          private readonly ILoggingService<OrderUpdateEventHandler> _logger;        
          private const string TopicName = "order_updated_exchange";
          private readonly IProducerServices _producerService;


          public OrderUpdateEventHandler(ILoggingService<OrderUpdateEventHandler> logger,  IProducerServices producerService)
          {
               _logger = logger;            
               _producerService = producerService;
          }

          public async Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
          {
               await _logger.LogInformationAsync($"Domain Event Handled: {notification.GetType().Name}");

               // Convert domain event to the DTO that you will publish
               var orderUpdatedIntegrationEvent = notification.order.ToOrderDTO();

               // Publish the event to Kafka
               var jsonMessage = JsonSerializer.Serialize(orderUpdatedIntegrationEvent);

               await _producerService.ProduceAsync(TopicName, jsonMessage);


               await _logger.LogInformationAsync("Order update event published to Kafka.");
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
          //                    Persistent = true,
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
