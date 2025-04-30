namespace Ordering.Application.Order.Queries.GetOrderByCustomer
{
   public record GetOrderByCustomerQuery(Guid CustomerId ) : IQuery<GetOrderByCustomerResult>;
   public record GetOrderByCustomerResult(IEnumerable<OrdersDTO> order);
  
}
