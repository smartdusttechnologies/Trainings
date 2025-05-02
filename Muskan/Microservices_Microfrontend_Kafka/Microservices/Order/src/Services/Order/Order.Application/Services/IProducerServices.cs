namespace Ordering.Application.Services
{
     public interface IProducerServices
     {
          Task ProduceAsync(string topic, string message, CancellationToken cancellationToken = default);
     }
}
