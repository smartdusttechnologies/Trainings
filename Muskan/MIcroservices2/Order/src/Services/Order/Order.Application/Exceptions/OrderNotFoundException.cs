namespace Ordering.Application.Exceptions
{
    /// <summary>
    /// Custom Exception for the Order Not found
    /// </summary>
    public class OrderNotFoundException : NotFoundException
    {
        /// <summary>
        /// With Order ID not found exception
        /// </summary>
        /// <param name="Id"></param>
        public OrderNotFoundException(Guid Id) : base("Order" , Id)
        {
            
        }
    }
}
