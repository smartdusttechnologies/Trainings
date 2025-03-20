
using MediatR;

namespace Ordering.Infrastructure.Data.Interceptors
{
    /// <summary>
    ///This interceptor ensures domain events are dispatched before database changes are committed.
    /// </summary>
    /// <param name="mediator"></param>
    public class DispatchDomainEventInterceptors(IMediator mediator) : SaveChangesInterceptor
    {
        /// <summary>
        /// Synchronously dispatch domain events before saving changes to the database.
        /// </summary>
        /// <param name="eventData"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
            return base.SavingChanges(eventData, result);
        }
        /// <summary>
        /// Asynchronous version of SavingChanges to handle domain events without blocking execution.
        /// </summary>
        /// <param name="eventData"></param>
        /// <param name="result"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
           await DispatchDomainEvents(eventData.Context);

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        /// <summary>
        /// Method responsible for collecting and dispatching domain events.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task DispatchDomainEvents(DbContext context)
        {
            if (context == null) return;  // Ensure context is valid
           // Get all aggregates that have domain events
            var aggregate = context.ChangeTracker
                .Entries<IAggregate>()
                .Where(a => a.Entity.DomainEvents.Any()) // Check for domain events
                .Select(a => a.Entity);
            var domainEvents = aggregate
                .SelectMany(a => a.DomainEvents)
                .ToList();
            aggregate.ToList().ForEach(a => a.ClearDomainEvents());
           foreach(var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }
        }
    }
}
