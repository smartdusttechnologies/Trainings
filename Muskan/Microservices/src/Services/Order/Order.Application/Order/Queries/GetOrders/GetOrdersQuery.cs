using BuildingBlock.Pagination;

namespace Ordering.Application.Order.Queries.GetOrders
{
   public record GetOrdersQuery(PaginateRequest PaginateRequest) : IQuery<GetOrderResult>;
   public record GetOrderResult(PaginateResult<OrdersDTO> PaginateRequest);
   
}
