using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace BuildingBlock.Messaging.Producer
{
     public abstract class RabbitMqProducerBase<T> where T : class
     {
          private readonly IConnectionFactory _connectionFactory;
          private readonly string _exchange;
          private readonly string _queue;

          protected RabbitMqProducerBase(IConnectionFactory factory, string exchange, string queue)
          {
               _connectionFactory = factory;
               _exchange = exchange;
               _queue = queue;
          }

          public async Task Publish(T message)
          {
               try
               {
                    using var connection = await _connectionFactory.CreateConnectionAsync();
                    Console.WriteLine("Connected to RabbitMQ.");
                    using var channel = await connection.CreateChannelAsync(); // Await the Task<IChannel> to get IChannel instance
                    Console.WriteLine("Channel created.");

                    await channel.ExchangeDeclareAsync(_exchange, ExchangeType.Fanout, durable: true);
                    await channel.QueueDeclareAsync(_queue, durable: true, exclusive: false, autoDelete: false);
                    await channel.QueueBindAsync(_queue, _exchange, "");
                    //await channel.ExchangeDeclareAsync(_exchange, ExchangeType.Fanout, durable: true); // Use the async method ExchangeDeclareAsync

                    var json = JsonSerializer.Serialize(message);
                    var body = Encoding.UTF8.GetBytes(json);

                    var properties = new BasicProperties
                    {
                         Persistent = true
                    };
                    await channel.BasicPublishAsync(
                         exchange: _exchange,
                         routingKey: "",
                         mandatory: false,
                         basicProperties: properties,
                         body: body); // Use the async method BasicPublishAsync
                    Console.WriteLine("Message published.");
               }
               catch (Exception ex)
               {
                    Console.WriteLine($"Error: {ex.Message}");
               }
          }
     }
}
