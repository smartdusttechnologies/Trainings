using Microsoft.EntityFrameworkCore;
using Ordering.Application.Extensions;
using Ordering.Domain.Abstraction;
using System.Data;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

namespace Ordering.Application.Order.Queries.GetOrderByName
{
    /// <summary>
    /// Get the order with order namme like ORD_004
    /// </summary>
    /// <param name="context"></param>
    public class GetOrderByNameHandler(IApplicationDbContext context) : IQueryHandler<GetOrderByNameQuery, GetOrderByNameResult>
    {
        /// <summary>
        /// Handles the query to retrieve an order by name.
        /// </summary>
        /// <param name="query">The query object containing the order name to search for.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation, returning GetOrderByNameResult.</returns>
        public async Task<GetOrderByNameResult> Handle(GetOrderByNameQuery query, CancellationToken cancellationToken)
        {
             // Find the orders matching the given order name from the database
            var orders = await context.Orders
                // Include associated order items in the query result
                .Include(o => o.OrderItems)
                // Disable entity tracking to improve performance for read-only queries
                .AsNoTracking()
                // Filter the orders where the OrderName contains the query name
                .Where(o => o.OrderName.Value.Contains(query.Name))
                // Order the results by OrderName for better readability
                .OrderBy(o => o.OrderName.Value)
                // Execute the query asynchronously and return the list of orders
                .ToListAsync(cancellationToken);

            // Convert the orders to DTO format and return the result
            return new GetOrderByNameResult(orders.ToOrderDtoList());
        }
    }
}
/// <summary>
/// What is AsNoTracking() in Entity Framework Core?
/// AsNoTracking() is a method in Entity Framework Core that improves performance by disabling change tracking for the retrieved entities.
/// </summary>

/// <summary>
/// How Does It Improve Performance?
/// Disables Change Tracking:
/// By default, EF Core tracks changes to entities so it can update them in the database later.
/// When querying data that is only for reading (not updating), this tracking is unnecessary and adds extra memory overhead.
/// AsNoTracking() tells EF Core not to track changes, reducing memory usage.
/// Speeds Up Query Execution:
/// EF Core doesn’t have to check for changes in the returned entities.
/// Queries run faster because EF skips maintaining tracking metadata.
/// Reduces Memory Consumption:
/// EF Core does not store the retrieved entities in its internal change tracker.
/// This is useful when handling large datasets.
/// </summary>

/// <summary>
/// When Should You Use AsNoTracking()?
/// When retrieving read-only data (e.g., reports, analytics, or search queries).
/// When you don’t need to update or save changes to the retrieved entities.
/// When working with large datasets where memory optimization is needed.
/// </summary>