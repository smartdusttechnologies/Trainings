namespace Ordering.Domain.Events;
/// <summary>
/// Event triggered when a new order is created.
/// </summary>
/// <param name="order"></param>
public record OrderCreatedEvent(Models.Orders order) : IDomainEvent;
   
