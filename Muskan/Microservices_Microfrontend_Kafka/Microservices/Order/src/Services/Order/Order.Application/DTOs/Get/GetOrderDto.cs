using BuildingBlock.Pagination;

namespace Ordering.Application.DTOs.Get
{
     public record GetOrderResponse(PaginateResult<OrdersDTO> PaginateResult);
}
