using System.Text;
using System.Text.Json;
using BuildingBlock.Messaging.Events;
using Microsoft.Extensions.Hosting;
using Ordering.Application.Order.Commands.CreateOrder;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Ordering.Application.Order.EventHandlers.Integration
{
     public class BasketCheckOutEventHandler : BackgroundService
     {
          private readonly IConnectionFactory _connectionFactory;
          private readonly string _exchange = "basket_checkout_exchange";
          private readonly string _queue = "basket_checkout_queue";
          private readonly ISender _sender;

          private readonly ILogger<BasketCheckOutEventHandler> _logger; // ILogger injected
          public BasketCheckOutEventHandler(IConnectionFactory connectionFactory, ILogger<BasketCheckOutEventHandler> logger, ISender sender)
          {
               _connectionFactory = connectionFactory;
               _logger = logger;  // Assign the injected logger
               _sender = sender;
          }

          protected override async Task ExecuteAsync(CancellationToken stoppingToken)
          {
               try
               {
                    var connection = await _connectionFactory.CreateConnectionAsync();
                    var channel = await connection.CreateChannelAsync();

                    _logger.LogInformation("Consumer: Connected to RabbitMQ.");

                    await channel.ExchangeDeclareAsync(_exchange, ExchangeType.Fanout, durable: true);
                    await channel.QueueDeclareAsync(_queue, durable: true, exclusive: false, autoDelete: false);
                    await channel.QueueBindAsync(_queue, _exchange, "");

                    var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.ReceivedAsync += async (model, ea) =>
                    {
                         try
                         {
                              var body = ea.Body.ToArray();
                              _logger.LogInformation($"Body from the RabbitMq : {body}");
                              var json = Encoding.UTF8.GetString(body);
                              _logger.LogInformation($"Json from the RabbitMq : {json}");
                              var message = JsonSerializer.Deserialize<BasketCheckOutEvents>(json);
                              _logger.LogInformation($"Message from the RabbitMq : {message}");
                              if (message != null)
                              {
                                   _logger.LogInformation("Received a message from RabbitMQ.");
                                   await HandleMessageAsync(message, stoppingToken); // Pass the CancellationToken here
                              }

                              await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                         }
                         catch (Exception ex)
                         {
                              await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
                              _logger.LogError($"Error occurred while processing message: {ex.Message}");
                         }
                    };

                    await channel.BasicConsumeAsync(queue: _queue, autoAck: false, consumer: consumer);
               }
               catch (Exception ex)
               {
                    _logger.LogError($"Consumer: Connection error: {ex.Message}");
               }
               await Task.CompletedTask;
          }


          // Method to handle the BasketCheckOutEvents
          protected virtual async Task HandleMessageAsync(BasketCheckOutEvents message, CancellationToken cancellationToken)
          {
               try
               {
                    _logger.LogInformation($"Handling message: {message?.ToString()}");

                    // Create order command from the event message
                    var command = MapToCreateOrderCommand(message);

                    // Send the CreateOrderCommand using ISender
                    await _sender.Send(command);

                    // Log the successful processing
                    _logger.LogInformation("Order creation command sent successfully.");
               }
               catch (Exception ex)
               {
                    _logger.LogError($"Error processing the message: {ex.Message}");
                    throw;
               }
          }
          private CreateOrderCommand MapToCreateOrderCommand(BasketCheckOutEvents message)
          {
               var addressDTO = new AddressDTO(message.FirstName, message.LastName, message.EmailAdress, message.AdressLine, message.Country, message.State, message.ZipCode);

               var paymentDto = new PaymentDTO(message.CardName, message.CardNumber, message.ExpiryDate, message.CVV, message.PaymentMethod);

               // Order Id 
               var orderId = Guid.NewGuid();

               var orderDto = new OrdersDTO(
                   Id: orderId,
                   CustomerId: message.CustomerId,
                   OrderName: message.Username,
                   ShippingAddress: addressDTO,
                   BillingAddress: addressDTO,
                   Payment: paymentDto,
                   Status: OrderStatus.Pending,
                   OrderItems:
                   [

                   new OrderItemDTO(orderId, message.CustomerId,  1, message.TotalPrice),
                        //new OrderItemDTO(orderId, new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"), 1, 400)
                   ]
                   );
               return new CreateOrderCommand(orderDto);
          }
     }

}
