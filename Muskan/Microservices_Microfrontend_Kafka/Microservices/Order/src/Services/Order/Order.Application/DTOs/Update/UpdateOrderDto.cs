namespace Ordering.Application.DTOs.Update
{
     public record UpdateOrderRequest(OrdersDTO Order);
     public record UpdateOrderResponse(bool IsSuccess);

}
