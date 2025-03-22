
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Abstraction;
using System;
using System.Diagnostics;
using System.Net.NetworkInformation;

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
        /// This method publishes all unhandled domain events from aggregate entities that are being tracked by EF Core.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task DispatchDomainEvents(DbContext context)
        {
            // Ensure context is valid
            if (context == null) return;  
            // Get all aggregates that have domain events
            var aggregate = context.ChangeTracker
                //Retrieves all tracked entities that implement the IAggregate interface.
                .Entries<IAggregate>()
                //Filters out only aggregates that have pending domain events.
                .Where(a => a.Entity.DomainEvents.Any()) 
                //Extracts the actual aggregate entity from Entries.
                .Select(a => a.Entity);
            //Collect All Domain Events
            var domainEvents = aggregate
                //Flattens all domain events from all aggregates into a single list.
                .SelectMany(a => a.DomainEvents)
                //Converts the result into a list for easier iteration.
                .ToList();
               //Clears the domain events from the aggregate entities before publishing.
               //This prevents duplicate dispatching of domain events if the method is called again.
            aggregate.ToList().ForEach(a => a.ClearDomainEvents());
            //Publish Each Domain Event Using mediator
           foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }
        }
    }
}
//Why Is This Necessary?
//Implements Domain-Driven Design (DDD) Principles

//Helps in enforcing the Separation of Concerns by decoupling domain logic from infrastructure concerns.

//Ensures Business Logic Execution After Transactions

//Domain events should be executed only after database transactions are committed.

//Helps in scenarios like sending emails, notifications, updating caches, etc.

//Prevents Side Effects in Transaction Handling

//If a database transaction fails, the associated domain events should not be published.

//This avoids inconsistent states in the system.

//Supports Event-Driven Architecture

//Useful for implementing CQRS (Command Query Responsibility Segregation) and Event Sourcing.

//Events can be handled asynchronously by other microservices.

//Enhances Testability & Maintainability

//The domain logic is now independent of the infrastructure layer.

//Makes it easier to unit test the domain logic.