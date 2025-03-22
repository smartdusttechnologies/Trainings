﻿namespace Ordering.Application.Order.EventHandlers
{
    public class OrderUpdateEventHandler(ILogger<OrderUpdateEventHandler> logger): INotificationHandler<OrderUpdatedEvent>
    {
        public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event Handled : {DomainEvent}", notification.GetType().Name);
            return Task.CompletedTask;
     }
    }
}
