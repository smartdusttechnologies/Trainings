namespace Ordering.Application.DTOs.Create
{
     public record CreateOrderRequest(OrdersDTO Order);
     public record CreateOrderResponse(Guid Id);

}
