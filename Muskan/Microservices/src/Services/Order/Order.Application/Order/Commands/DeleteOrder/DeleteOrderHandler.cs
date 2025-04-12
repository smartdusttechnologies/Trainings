namespace Ordering.Application.Order.Commands.DeleteOrder
{
    /// <summary>
    /// Handler for the DeleteOrderCommand
    /// </summary>
    public class DeleteOrderHandler(IApplicationDbContext context) : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
    {
        /// <summary>
        /// Handles the delete order command
        /// </summary>
        /// <param name="command">The delete order command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The result of the delete order command</returns>
        public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            //Get the OrderID from the Value Object of OrderID 
            var orderId = OrderId.Of(command.orderId);
            //Find the order from the OrderID command
            var order = await context.Orders
                //FindAsync : find an entity with the given primary key values. 
                .FindAsync([orderId], cancellationToken);
            //If order is null it throw custom exception 
            if (order == null) {
                throw new OrderNotFoundException(command.orderId);
            }
            //Remove the order with given orderID
            context.Orders.Remove(order);
            //Save it to the database
            await context.SaveChangesAsync(cancellationToken);
            //return the result as true 
            return new DeleteOrderResult(true);
        }
    }
}
