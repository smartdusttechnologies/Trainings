namespace Ordering.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for OrderItem
    /// </summary>
    /// <param name="OrderId">The ID of the order</param>
    /// <param name="ProductId">The ID of the product</param>
    /// <param name="Quantity">The quantity of the product</param>
    /// <param name="Price">The price of the product</param>
    public record OrderItemDTO
    (
        Guid OrderId,
        Guid ProductId,
        int Quantity,
        decimal Price
    );
}
