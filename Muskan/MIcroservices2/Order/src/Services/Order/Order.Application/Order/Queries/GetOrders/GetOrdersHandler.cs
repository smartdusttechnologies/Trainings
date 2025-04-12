using BuildingBlock.Pagination;
using System;
namespace Ordering.Application.Order.Queries.GetOrders
{
    /// <summary>
    /// Handler for processing GetOrdersQuery and returning paginated order results
    /// </summary>
    /// <param name="context"></param>
    public class GetOrdersHandler(IApplicationDbContext context) : IQueryHandler<GetOrdersQuery, GetOrderResult>
    {
        /// <summary>
        ///  Handles the query and returns paginated results
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<GetOrderResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
        {
            // Extracting pagination Index from the request
            var pageIndex = query.PaginateRequest.PageIndex;
            // Extracting pagination Size from the request
            var pageSize = query.PaginateRequest.PageSize;
            // Fetching the total count of orders for pagination metadata
            var totalCount = await context.Orders.LongCountAsync(cancellationToken);
            // Retrieving paginated list of orders with related order items
            var orders = await context.Orders
                // Including order items for each order
                .Include(o => o.OrderItems)
                // Ordering by OrderName
                .OrderBy(o => o.OrderName.Value)
                // Skipping records based on the page index
                .Skip(pageSize * pageIndex)
                // Taking only the required number of records
                .Take(pageSize)
                .ToListAsync(cancellationToken);
            // Returning the paginated result with order data converted to DTOs
            return new GetOrderResult(
                new PaginateResult<OrdersDTO>(
                    // Current page index
                    pageIndex,
                    // Number of items per page
                    pageSize,
                    // Total count of records
                    totalCount,
                    // Mapping orders to DTOs
                    orders.ToOrderDtoList() 
                    ));


        }
    }
}
//1.LongCountAsync()
//What it does:
//LongCountAsync() is an asynchronous method that returns the total number of records in a table.
//It is similar to CountAsync(), but it returns a long instead of an int, which is useful when dealing with very large datasets.

//Why is it used?
//To determine the total number of orders before pagination. This helps in calculating the total number of pages available.


//2.Skip(int count)
//What it does:
//Skip(count) skips a specified number of rows from the result set.
//Used for pagination, ensuring that only the required subset of records is retrieved.

//Why is it used?
//To skip records that belong to previous pages, so only the current page’s data is fetched.
//Example : 
//.Skip(pageSize * pageIndex)
//If pageIndex = 2 and pageSize = 10, then Skip(20) will ignore the first 20 records.


//Take(int count)
//What it does:
//Take(count) selects only a limited number of rows from the result set.
//Works together with Skip() to fetch the correct page of data.

//Why is it used?
//To limit the number of results returned per page.
//Example : 
//.Take(pageSize)
//If pageSize = 10, only 10 records will be retrieved.

//4. Include(Expression<Func<T, object>> path)
//What it does:
//Include() is used to eagerly load related data from a foreign table.
//Prevents the "N+1 query problem" by fetching related entities in a single query.

//Why is it used?
//To ensure that OrderItems are loaded together with Orders, avoiding additional queries.
//Example : 
//.Include(o => o.OrderItems)
//Ensures that each order includes its related OrderItems instead of querying them separately.




