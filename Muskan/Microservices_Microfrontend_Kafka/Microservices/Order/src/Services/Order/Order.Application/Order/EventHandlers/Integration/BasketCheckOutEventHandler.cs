using System.Text.Json;
using BuildingBlock.Messaging.Events;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ordering.Application.Order.Commands.CreateOrder;

namespace Ordering.Application.Order.EventHandlers.Integration
{
     public class BasketCheckOutEventHandler : BackgroundService
     {

          private readonly IServiceScopeFactory _scopeFactory;
          private readonly ConsumerConfig _consumerConfig;

          public BasketCheckOutEventHandler(IConfiguration configuration, IServiceScopeFactory scopeFactory)
          {

               _scopeFactory = scopeFactory;
               var kafkaConfig = configuration.GetSection("Kafka");
               _consumerConfig = new ConsumerConfig
               {
                    BootstrapServers = kafkaConfig["BootstrapServers"],  // Address of the Kafka broker
                    GroupId = "basket-checkout-consumer-group",// Consumer group ID for load balancing
                    AutoOffsetReset = AutoOffsetReset.Earliest, // Start reading from the earliest message if no offset is committed
                    EnableAutoCommit = true // Automatically commit offsets after consuming messages
               };
          }
          protected override async Task ExecuteAsync(CancellationToken stoppingToken)
          {
               using var scope = _scopeFactory.CreateScope();
               var logger = scope.ServiceProvider.GetRequiredService<ILoggingService<BasketCheckOutEventHandler>>();
               var sender = scope.ServiceProvider.GetRequiredService<ISender>();

               using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig)
                   .SetErrorHandler((_, e) => logger.LogErrorAsync($"Kafka Consumer Error: {e.Reason}", new Exception(e.Reason)))
                   .Build();

               consumer.Subscribe("basket_checkout_topic");
               await logger.LogInformationAsync("Consumer subscribed to topic: basket_checkout_topic");

               try
               {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                         try
                         {
                              var result = consumer.Consume(stoppingToken);
                              if (result != null)
                              {
                                   var message = JsonSerializer.Deserialize<BasketCheckOutEvents>(result.Message.Value);

                                   if (message != null)
                                   {
                                        await logger.LogInformationAsync("Order creation message sent to handler.");
                                        var command = MapToCreateOrderCommand(message);
                                        await sender.Send(command);
                                        await logger.LogInformationAsync("Order creation command sent successfully.");
                                   }
                                   else
                                   {
                                        await logger.LogWarningAsync("Received null or invalid log entry message.");
                                   }
                              }
                         }
                         catch (ConsumeException ex)
                         {
                              await logger.LogErrorAsync($"Consume error: {ex.Error.Reason}", ex);
                         }
                    }
               }
               catch (OperationCanceledException)
               {
                    await logger.LogInformationAsync("Consumer cancellation requested. Exiting...");
               }
               catch (Exception ex)
               {
                    await logger.LogCriticalAsync($"Unhandled exception in consumer loop: {ex}", ex);
               }
               finally
               {
                    consumer.Close();
                    await logger.LogInformationAsync("Kafka consumer closed.");
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





//// Method to handle the BasketCheckOutEvents
//protected virtual async Task HandleMessageAsync(BasketCheckOutEvents message, CancellationToken cancellationToken)
//          {
//               try
//               {
//                    _logger.LogInformation($"Handling message: {message?.ToString()}");

//                    // Create order command from the event message
//                    var command = MapToCreateOrderCommand(message);

//                    // Send the CreateOrderCommand using ISender
//                    await _sender.Send(command);

//                    // Log the successful processing
//                    _logger.LogInformation("Order creation command sent successfully.");
//               }
//               catch (Exception ex)
//               {
//                    _logger.LogError($"Error processing the message: {ex.Message}");
//                    throw;
//               }
//          }

