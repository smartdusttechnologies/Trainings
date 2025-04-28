namespace Ordering.Domain.Models
{
    /// <summary>
    /// Represents an order item Entity.
    /// </summary>
    public class OrderItem : Entity<OrderItemId>
    {
        /// <summary>
        /// Nake Contructor of it
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        public OrderItem(OrderId orderId, ProductId productId, int quantity, decimal price)
        {
            Id = OrderItemId.Of(Guid.NewGuid());
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }
        /// <summary>
        /// OrderId
        /// </summary>
        public OrderId OrderId { get; private set; } = default;
        /// <summary>
        /// ProductId
        /// </summary>
        public ProductId ProductId { get; private set; } = default;
        /// <summary>
        /// Price
        /// </summary>
        public decimal Price { get; private set; } = default;
        /// <summary>
        /// Quantity
        /// </summary>
        public int Quantity { get; private set; } = default;

    }
}
