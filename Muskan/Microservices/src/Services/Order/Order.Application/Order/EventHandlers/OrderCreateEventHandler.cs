
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Order.EventHandlers
{
    public class OrderCreateEventHandler(ILogger<OrderCreateEventHandler> logger) : INotificationHandler<OrderCreatedEvent>
    {
        public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event Handled : {DomainEvent}", notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
