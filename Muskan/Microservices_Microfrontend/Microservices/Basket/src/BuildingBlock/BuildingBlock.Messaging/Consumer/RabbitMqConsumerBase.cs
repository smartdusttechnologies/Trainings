using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BuildingBlock.Messaging.Consumer
{
     public abstract class RabbitMqConsumerBase<T> where T : class
     {
          protected readonly IConnectionFactory _connectionFactory;
          protected readonly string _exchange;
          protected readonly string _queue;

          protected RabbitMqConsumerBase(IConnectionFactory factory, string exchange, string queue)
          {
               _connectionFactory = factory;
               _exchange = exchange;
               _queue = queue;
          }

          public async Task StartAsync()
          {
               var connection = await _connectionFactory.CreateConnectionAsync();
               var channel = await connection.CreateChannelAsync();

               await channel.ExchangeDeclareAsync(_exchange, ExchangeType.Fanout, durable: true);
               await channel.QueueDeclareAsync(_queue, durable: true, exclusive: false, autoDelete: false);
               await channel.QueueBindAsync(_queue, _exchange, "");

               var consumer = new AsyncEventingBasicConsumer(channel);
               consumer.ReceivedAsync += async (model, ea) =>
               {
                    try
                    {
                         var body = ea.Body.ToArray();
                         var json = Encoding.UTF8.GetString(body);
                         var message = JsonSerializer.Deserialize<T>(json);

                         if (message != null)
                              await HandleMessageAsync(message);

                         await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                    }
                    catch (Exception ex)
                    {
                         await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
                         Console.WriteLine($"Exception occurred: {ex.Message}");
                    }
               };

               channel.BasicConsumeAsync(queue: _queue, autoAck: false, consumer: consumer);

          }

          protected abstract Task HandleMessageAsync(T message);
     }
}
