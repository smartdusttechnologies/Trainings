﻿using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace Ordering.Application.Services
{
     // Kafka Overview:
     // Apache Kafka is a distributed event streaming platform used for building real-time data pipelines and streaming applications.
     // Key components used here:
     // - Broker: A Kafka server that stores and serves messages.
     // - Topic: A category to which messages are sent by producers and from which messages are consumed by consumers.
     // - Producer: An application that sends messages to a Kafka topic.

     public class ProducerServices : IProducerServices
     {
          private readonly IProducer<Null, string> producer;
          private readonly ILogger<ProducerServices> logger;
          public ProducerServices(ILogger<ProducerServices> _logger, IConfiguration configuration)
          {
               logger = _logger;
               var kafkaConfig = configuration.GetSection("Kafka");
               // Kafka producer configuration
               var config = new ProducerConfig
               {
                    BootstrapServers = kafkaConfig["BootstrapServers"], // Address of the Kafka broker
                    AllowAutoCreateTopics = true, // Automatically create topics if they don't exist
                    Acks = Acks.All, // Wait for all replicas to acknowledge the message
                    MessageSendMaxRetries = 3, // Maximum number of retries for sending a message
                    RetryBackoffMs = 100 // Time to wait before retrying
               };
               logger.LogInformation("Initializing Kafka producer...");

               try
               {
                    // Create a Kafka producer with the specified configuration
                    producer = new ProducerBuilder<Null, string>(config)
                        .SetErrorHandler((_, e) =>
                        {
                             logger.LogError("Kafka error: {Reason}", e.Reason);
                        })
                        .Build();

                    _logger.LogInformation("Kafka producer created. Awaiting first message to establish connection...");
               }
               catch (Exception ex)
               {
                    logger.LogCritical(ex, "Failed to initialize Kafka producer.");
                    throw;
               }

               // producer = new ProducerBuilder<Null, string>(config).SetErrorHandler((_, e) =>
               //{
               //     // Log any errors encountered by the producer
               //     logger.LogError("Kafka Producer Error: {Error}", e.Reason);
               //}).Build();
          }
          // Asynchronous method to produce messages to a Kafka topic
          public async Task ProduceAsync(string topic, string message, CancellationToken cancellationToken = default)
          {
               try
               {
                    logger.LogInformation($"Kafka message send to topic {topic}");
                    var result = await producer.ProduceAsync(topic, new Message<Null, string> { Value = message }, cancellationToken);
                    logger.LogInformation("Kafka message sent to {Offset}", result.TopicPartitionOffset);

               }
               catch (ProduceException<Null, string> ex)
               {
                    // Log any errors encountered during message production
                    logger.LogError("Delivery Failed: {Reason}", ex.Error.Reason);
               }
               catch (OperationCanceledException)
               {
                    // Handle cancellation of the message production
                    logger.LogWarning("Message production was canceled");
               }
               catch (Exception ex)
               {
                    // Log any unhandled exceptions
                    logger.LogCritical(ex, "Unexpected error occurred while producing message");
               }
               finally
               {
                    // Ensure the producer flushes any remaining messages
                    producer.Flush(cancellationToken);
                    logger.LogInformation("Producer flushed.");
               }
          }
     }
}


