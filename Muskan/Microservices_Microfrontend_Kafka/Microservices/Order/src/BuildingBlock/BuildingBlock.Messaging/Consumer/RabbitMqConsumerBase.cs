using System.Text;
using System.Text.Json;
using BuildingBlock.Messaging.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BuildingBlock.Messaging.Consumer
{
     public class RabbitMqConsumerBackgroundService : BackgroundService
     {
          private readonly IConnectionFactory _connectionFactory;
          private readonly string _exchange = "basket_checkout_exchange";
          private readonly string _queue = "basket_checkout_queue";

          private readonly ILogger<RabbitMqConsumerBackgroundService> _logger; // ILogger injected
          public RabbitMqConsumerBackgroundService(IConnectionFactory connectionFactory, ILogger<RabbitMqConsumerBackgroundService> logger)
          {
               _connectionFactory = connectionFactory;
               _logger = logger;  // Assign the injected logger
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
          }

          // Corrected Signature
          protected virtual Task HandleMessageAsync(BasketCheckOutEvents message, CancellationToken cancellationToken)
          {
               _logger.LogInformation($"Handling message: {message?.ToString()}");
               return Task.CompletedTask;
          }

     }
}
//using System.Text;
//using System.Text.Json;
//using Microsoft.Extensions.Hosting;
//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;
//namespace BuildingBlock.Messaging.Consumer
//{
//     public abstract class RabbitMqConsumerBase<T> : BackgroundService where T : class
//     {
//          protected readonly IConnectionFactory _connectionFactory;
//          protected readonly string _exchange;
//          protected readonly string _queue;

//          protected RabbitMqConsumerBase(IConnectionFactory factory, string exchange, string queue)
//          {
//               _connectionFactory = factory;
//               _exchange = exchange;
//               _queue = queue;
//          }

//          // This method is called by the background service when it starts
//          protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//          {
//               using var connection = await _connectionFactory.CreateConnectionAsync();
//               using var channel = await connection.CreateChannelAsync();

//               // Declare the exchange and queue for RabbitMQ
//               await channel.ExchangeDeclareAsync(_exchange, ExchangeType.Fanout, durable: true);
//               await channel.QueueDeclareAsync(_queue, durable: true, exclusive: false, autoDelete: false);
//               await channel.QueueBindAsync(_queue, _exchange, "");

//               var consumer = new AsyncEventingBasicConsumer(channel);
//               consumer.ReceivedAsync += async (model, ea) =>
//               {
//                    try
//                    {
//                         var body = ea.Body.ToArray();
//                         var json = Encoding.UTF8.GetString(body);
//                         var message = JsonSerializer.Deserialize<T>(json);

//                         if (message != null)
//                              await HandleMessageAsync(message);

//                         await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
//                    }
//                    catch (Exception ex)
//                    {
//                         await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
//                         Console.WriteLine($"Exception occurred: {ex.Message}");
//                    }
//               };

//               await channel.BasicConsumeAsync(queue: _queue, autoAck: false, consumer: consumer);

//               // Wait for cancellation
//               await Task.Delay(-1, stoppingToken); // Keeps the service running until canceled
//          }

//          protected abstract Task HandleMessageAsync(T message);
//     }
//}


//using System.Text;
//using System.Text.Json;
//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;

//namespace BuildingBlock.Messaging.Consumer
//{
//     public abstract class RabbitMqConsumerBase<T> where T : class
//     {
//          protected readonly IConnectionFactory _connectionFactory;
//          protected readonly string _exchange;
//          protected readonly string _queue;

//          protected RabbitMqConsumerBase(IConnectionFactory factory, string exchange, string queue)
//          {
//               _connectionFactory = factory;
//               _exchange = exchange;
//               _queue = queue;
//          }

//          public virtual async Task StartAsync()
//          {
//               try
//               {


//                    var connection = await _connectionFactory.CreateConnectionAsync();
//                    Console.WriteLine("Connected to RabbitMQ.");
//                    var channel = await connection.CreateChannelAsync();
//                    Console.WriteLine("Channel created.");
//                    await channel.ExchangeDeclareAsync(_exchange, ExchangeType.Fanout, durable: true);
//                    await channel.QueueDeclareAsync(_queue, durable: true, exclusive: false, autoDelete: false);
//                    await channel.QueueBindAsync(_queue, _exchange, "");

//                    var consumer = new AsyncEventingBasicConsumer(channel);
//                    consumer.ReceivedAsync += async (model, ea) =>
//                    {
//                         try
//                         {
//                              var body = ea.Body.ToArray();
//                              var json = Encoding.UTF8.GetString(body);
//                              var message = JsonSerializer.Deserialize<T>(json);

//                              if (message != null)
//                                   await HandleMessageAsync(message);

//                              await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
//                         }
//                         catch (Exception ex)
//                         {
//                              await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
//                              Console.WriteLine($"Exception occurred: {ex.Message}");
//                         }
//                    };

//                    await channel.BasicConsumeAsync(queue: _queue, autoAck: false, consumer: consumer);
//               }
//               catch (Exception ex)
//               {
//                    Console.WriteLine($"Error: {ex.Message}");
//               }
//          }

//          protected abstract Task HandleMessageAsync(T message);
//     }
//}



//using System.Text;
//using System.Text.Json;
//using Microsoft.Extensions.Hosting;
//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;

//namespace BuildingBlock.Messaging.Consumer
//{
//     public abstract class RabbitMqConsumerBase<T> : IHostedService where T : class
//     {
//          protected readonly IConnectionFactory _connectionFactory;
//          protected readonly string _exchange;
//          protected readonly string _queue;
//          private IConnection _connection;
//          private IChannel _channel; // Move channel to a class-level variable
//          private Task _consumerTask;
//          private CancellationTokenSource _cts;

//          protected RabbitMqConsumerBase(IConnectionFactory factory, string exchange, string queue)
//          {
//               _connectionFactory = factory;
//               _exchange = exchange;
//               _queue = queue;
//          }

//          public async Task StartAsync(CancellationToken cancellationToken)
//          {
//               // Initialize the CancellationTokenSource to support graceful shutdown
//               _cts = new CancellationTokenSource();

//               // Create the connection and channel asynchronously
//               _connection = await _connectionFactory.CreateConnectionAsync();
//               _channel = await _connection.CreateChannelAsync(); // Now stored as a class-level variable

//               // Declare the exchange and queue
//               await _channel.ExchangeDeclareAsync(_exchange, ExchangeType.Fanout, durable: true);
//               await _channel.QueueDeclareAsync(_queue, durable: true, exclusive: false, autoDelete: false);
//               await _channel.QueueBindAsync(_queue, _exchange, "");

//               var consumer = new AsyncEventingBasicConsumer(_channel);
//               consumer.ReceivedAsync += async (model, ea) =>
//               {
//                    try
//                    {
//                         var body = ea.Body.ToArray();
//                         var json = Encoding.UTF8.GetString(body);
//                         var message = JsonSerializer.Deserialize<T>(json);

//                         if (message != null)
//                              await HandleMessageAsync(message);

//                         // Acknowledge the message
//                         await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
//                    }
//                    catch (Exception ex)
//                    {
//                         // Negative acknowledgement (requeue) on error
//                         await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
//                         Console.WriteLine($"Exception occurred: {ex.Message}");
//                    }
//               };

//               // Start consuming the messages
//               _consumerTask = Task.Run(() => _channel.BasicConsumeAsync(_queue, autoAck: false, consumer: consumer), cancellationToken);
//          }

//          public async Task StopAsync(CancellationToken cancellationToken)
//          {
//               // Gracefully close the consumer and the connection
//               _cts.Cancel();
//               await _consumerTask;

//               // Ensure the channel and connection are closed properly
//               await _channel.CloseAsync(); // Now accessible
//               await _connection.CloseAsync();
//          }

//          protected abstract Task HandleMessageAsync(T message);
//     }
//}




//using System.Text;
//using System.Text.Json;
//using Microsoft.Extensions.Hosting;
//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;

//namespace BuildingBlock.Messaging.Consumer
//{
//     public abstract class RabbitMqConsumerBase<T> : IHostedService where T : class
//     {
//          protected readonly IConnectionFactory _connectionFactory;
//          protected readonly string _exchange;
//          protected readonly string _queue;
//          private IConnection _connection;
//          private IChannel _channel; // Move channel to a class-level variable
//          private Task _consumerTask;
//          private CancellationTokenSource _cts;

//          protected RabbitMqConsumerBase(IConnectionFactory factory, string exchange, string queue)
//          {
//               _connectionFactory = factory;
//               _exchange = exchange;
//               _queue = queue;
//          }

//          public async Task StartAsync(CancellationToken cancellationToken)
//          {
//               // Initialize the CancellationTokenSource to support graceful shutdown
//               _cts = new CancellationTokenSource();

//               // Create the connection and channel asynchronously
//               _connection = await _connectionFactory.CreateConnectionAsync();
//               _channel = await _connection.CreateChannelAsync(); // Now stored as a class-level variable

//               // Declare the exchange and queue
//               await _channel.ExchangeDeclareAsync(_exchange, ExchangeType.Fanout, durable: true);
//               await _channel.QueueDeclareAsync(_queue, durable: true, exclusive: false, autoDelete: false);
//               await _channel.QueueBindAsync(_queue, _exchange, "");

//               var consumer = new AsyncEventingBasicConsumer(_channel);
//               consumer.ReceivedAsync += async (model, ea) =>
//               {
//                    try
//                    {
//                         var body = ea.Body.ToArray();
//                         var json = Encoding.UTF8.GetString(body);
//                         var message = JsonSerializer.Deserialize<T>(json);

//                         if (message != null)
//                              await HandleMessageAsync(message);

//                         // Acknowledge the message
//                         await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
//                    }
//                    catch (Exception ex)
//                    {
//                         // Negative acknowledgement (requeue) on error
//                         await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
//                         Console.WriteLine($"Exception occurred: {ex.Message}");
//                    }
//               };

//               // Start consuming the messages
//               _consumerTask = Task.Run(() => _channel.BasicConsumeAsync(_queue, autoAck: false, consumer: consumer), cancellationToken);
//          }

//          public async Task StopAsync(CancellationToken cancellationToken)
//          {
//               // Gracefully close the consumer and the connection
//               _cts.Cancel();
//               await _consumerTask;

//               // Ensure the channel and connection are closed properly
//               await _channel.CloseAsync(); // Now accessible
//               await _connection.CloseAsync();
//          }

//          protected abstract Task HandleMessageAsync(T message);
//     }
//}

