namespace Ordering.Application.Order.Queries.GetOrderByCustomer
{
     /// <summary>
     /// Get Order by the Customer Id( which is GUID)
     /// </summary>
     /// <param name="context"></param>
     public class GetOrderByCustomerHandler(IApplicationDbContext context, ILoggingService<GetOrderByCustomerHandler> logger) : IQueryHandler<GetOrderByCustomerQuery, GetOrderByCustomerResult>
     {
          /// <summary>
          /// 
          /// </summary>
          /// <param name="query"></param>
          /// <param name="cancellationToken"></param>
          /// <returns></returns>
          public async Task<GetOrderByCustomerResult> Handle(GetOrderByCustomerQuery query, CancellationToken cancellationToken)
          {
               await logger.LogInformationAsync($"Get order by query {query}");
               ///Find the order with customer id 
               var orders = await context.Orders
                          .Include(o => o.OrderItems)
               .AsNoTracking()
                          .Where(o => o.CustomerId == CustomerId.Of(query.CustomerId))
                          .OrderBy(o => o.OrderName.Value)
                          .ToListAsync(cancellationToken);
               await logger.LogInformationAsync($"Order :  {orders.ToOrderDtoList()}");
               /// return the list of customer with extension  method  ToOrderDtoList()
               return new GetOrderByCustomerResult(orders.ToOrderDtoList());
          }
     }
}
