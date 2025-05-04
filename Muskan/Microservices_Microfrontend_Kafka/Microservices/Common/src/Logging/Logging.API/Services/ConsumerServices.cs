using System.Text.Json;
using BuildingBlock.Messaging.Events;
using Confluent.Kafka;
using Logging.API.Data;

namespace Logging.API.Services
{
     // Kafka Overview:
     // Apache Kafka is a distributed event streaming platform used for building real-time data pipelines and streaming applications.
     // Key components used here:
     // - Broker: A Kafka server that stores and serves messages.
     // - Topic: A category to which messages are sent by producers and from which messages are consumed by consumers.
     // - Consumer: An application that reads messages from a Kafka topic.
     // - Consumer Group: A group of consumers that work together to consume messages from a topic.

     public class ConsumerServices : BackgroundService
     {
          private readonly ILogger<ConsumerServices> _logger;
          private readonly IServiceScopeFactory _scopeFactory;
          private readonly ConsumerConfig _consumerConfig;

          public ConsumerServices(ILogger<ConsumerServices> logger,
              IServiceScopeFactory scopeFactory ,IConfiguration configuration)
          {
               _logger = logger; 
               _scopeFactory = scopeFactory;
               var kafkaConfig = configuration.GetSection("Kafka");
               _consumerConfig = new ConsumerConfig
               {
                    BootstrapServers = kafkaConfig["BootstrapServers"], // Address of the Kafka broker
                    GroupId = "log-consumer-group",// Consumer group ID for load balancing
                    AutoOffsetReset = AutoOffsetReset.Earliest, // Start reading from the earliest message if no offset is committed
                    EnableAutoCommit = true // Automatically commit offsets after consuming messages
               };
          }
          // The main execution method for the background service.
          protected override async Task ExecuteAsync(CancellationToken stoppingToken)
          {

               // Create a Kafka consumer with the specified configuration
               using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).SetErrorHandler((_, e) =>
               {
                    // Log any errors encountered by the consumer
                    _logger.LogError("Kafka Consumer Error: {Error}", e.Reason);
               }).Build();

               // Subscribe the consumer to the specified topic
               consumer.Subscribe("log-entry-topic");
               _logger.LogInformation("Consumer subscribed to topic: log-entry-topic");

               try
               {
                    // Continuously consume messages until cancellation is requested
                    while (!stoppingToken.IsCancellationRequested)
                    {
                         try
                         {
                              // Consume a message from the topic
                              var result = consumer.Consume(stoppingToken);
                              if (result != null)
                              {
                                   var messageJson = result.Message.Value;
                                   var message = JsonSerializer.Deserialize<LogEntryMessage>(messageJson);

                                   if (message != null)
                                   {
                                        using var scope = _scopeFactory.CreateScope();
                                        var logService = scope.ServiceProvider.GetRequiredService<ILogEntryServices>();
                                        await logService.SaveLogs(message);
                                   }
                                   else
                                   {
                                        _logger.LogWarning("Received null or invalid log entry message.");
                                   }
                              }
                         }
                         catch (ConsumeException ex)
                         {
                              // Log any errors encountered during message consumption
                              _logger.LogError("Consume error: {Error}", ex.Error.Reason);
                         }
                    }
               }
               catch (OperationCanceledException)
               {
                    // Handle cancellation of the consumer loop
                    _logger.LogInformation("Consumer cancellation requested. Exiting...");
               }
               catch (Exception ex)
               {
                    // Log any unhandled exceptions
                    _logger.LogCritical(ex, "Unhandled exception in consumer loop");
               }
               finally
               {
                    // Ensure the consumer is closed properly
                    consumer.Close();
                    _logger.LogInformation("Kafka consumer closed.");
               }
          }
     }
}
