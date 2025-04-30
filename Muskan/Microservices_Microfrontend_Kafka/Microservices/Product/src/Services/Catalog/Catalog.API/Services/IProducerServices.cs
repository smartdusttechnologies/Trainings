namespace Catalog.API.Services
{
     public interface IProducerServices
     {
          Task ProduceAsync(string topic, string message, CancellationToken cancellationToken = default);
     }
}
