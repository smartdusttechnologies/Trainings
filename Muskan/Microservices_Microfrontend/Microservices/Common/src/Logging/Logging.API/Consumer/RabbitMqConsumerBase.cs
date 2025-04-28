//namespace Logging.API.Consumer
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

//          public void Start()
//          {
//               var connection = _connectionFactory.CreateConnection();
//               var channel = connection.CreateModel();

//               channel.ExchangeDeclare(_exchange, ExchangeType.Fanout, durable: true);
//               channel.QueueDeclare(_queue, durable: true, exclusive: false, autoDelete: false);
//               channel.QueueBind(_queue, _exchange, "");

//               var consumer = new EventingBasicConsumer(channel);
//               consumer.Received += async (model, ea) =>
//               {
//                    try
//                    {
//                         var body = ea.Body.ToArray();
//                         var json = Encoding.UTF8.GetString(body);
//                         var message = JsonSerializer.Deserialize<T>(json);

//                         if (message != null)
//                              await HandleMessageAsync(message);

//                         channel.BasicAck(ea.DeliveryTag, multiple: false);
//                    }
//                    catch (Exception ex)
//                    {
//                         // optionally: retry or dead-letter
//                         channel.BasicNack(ea.DeliveryTag, false, requeue: true);
//                    }
//               };

//               channel.BasicConsume(_queue, autoAck: false, consumer: consumer);
//          }

//          protected abstract Task HandleMessageAsync(T message);
//     }

//}
