namespace Ordering.Application.DTOs
{
    public record OrdersDTO
    (
            Guid Id,
            Guid CustomerId,
            string OrderName ,
            AddressDTO ShippingAddress,
            AddressDTO BillingAddress,
            PaymentDTO Payment,
            OrderStatus Status,
            List<OrderItemDTO> OrderItems

        );
       

    
}
