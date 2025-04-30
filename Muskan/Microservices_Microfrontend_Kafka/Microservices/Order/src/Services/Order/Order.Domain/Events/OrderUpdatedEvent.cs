namespace Ordering.Domain.Events;
/// <summary>
/// Event triggered when an existing order is updated.
/// </summary>
/// <param name="order"></param>
public record OrderUpdatedEvent(Models.Orders order) : IDomainEvent;

