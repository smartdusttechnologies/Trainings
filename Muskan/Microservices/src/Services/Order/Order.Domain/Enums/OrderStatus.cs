namespace Ordering.Domain.Enums
{
    /// <summary>
    /// Order status enumeration
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Draft order status
        /// </summary>
        Draft = 1,
        /// <summary>
        /// pending order status
        /// </summary>
        Pending = 2,
        /// <summary>
        /// Completed order status
        /// </summary>
        Completed = 3,
        /// <summary>
        /// Cancelled order status
        /// </summary>
        Cancelled = 4
    }
}
