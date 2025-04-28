namespace Ordering.Application.DTOs.Get
{
     public record GetOrderByCustomerResponse(IEnumerable<OrdersDTO> Orders);
}
