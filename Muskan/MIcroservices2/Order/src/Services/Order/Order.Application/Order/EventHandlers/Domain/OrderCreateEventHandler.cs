using MassTransit;
using Microsoft.FeatureManagement;

namespace Ordering.Application.Order.EventHandlers.Domain
{
     public class OrderCreateEventHandler(IPublishEndpoint publishEndpoint, IFeatureManager featureManager, ILoggingService<OrderCreateEventHandler> logger) : INotificationHandler<OrderCreatedEvent>
     {
          public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
          {
               await logger.LogInformationAsync($"Domain Event Handled : {domainEvent.GetType().Name}");
               if (await featureManager.IsEnabledAsync("OrderFullFillment"))
               {
                    var orderCreatedIntegraionEvent = domainEvent.order.ToOrderDTO(); ;
                    await publishEndpoint.Publish(orderCreatedIntegraionEvent, cancellationToken);
               }
          }
     }
}
