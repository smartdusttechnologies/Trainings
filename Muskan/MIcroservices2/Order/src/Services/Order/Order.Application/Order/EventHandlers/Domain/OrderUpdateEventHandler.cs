namespace Ordering.Application.Order.EventHandlers.Domain
{
     public class OrderUpdateEventHandler(ILoggingService<OrderUpdateEventHandler> logger) : INotificationHandler<OrderUpdatedEvent>
     {
          public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
          {
               logger.LogInformationAsync($"Domain Event Handled : {notification.GetType().Name}");
               return Task.CompletedTask;
          }
     }
}
