namespace Ordering.Application.Order.Commands.CreateOrder
{  
    /// <summary>
    /// Handler for the CreateOrderCommand
    /// </summary>
    /// <param name="context"></param>
    public class CreateOrderHandler(IApplicationDbContext context) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
    {
        // Method to handle the CreateOrderCommand
        public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            // Create Order Entity from Command Object
            var order = CreateNewOrder(command.Order);

            // Add the new order to the context
            context.Orders.Add(order);

            // Save changes to the database
            await context.SaveChangesAsync(cancellationToken);

            // Return the result with the new order ID
            return new CreateOrderResult(order.Id.Value);
        }

        // Method to create a new order from the OrdersDTO
        private Orders CreateNewOrder(OrdersDTO OrderDTO)
        {
            // Create the shipping address from the OrdersDTO
            var shippingAddress = Address.Of(OrderDTO.ShippingAddress.Firstname, OrderDTO.ShippingAddress.Lastname, OrderDTO.ShippingAddress.EmailAdress, OrderDTO.ShippingAddress.AddressLine, OrderDTO.ShippingAddress.Country, OrderDTO.ShippingAddress.State, OrderDTO.ShippingAddress.ZipCode);

            // Create the billing address from the OrdersDTO
            var billingAddress = Address.Of(OrderDTO.BillingAddress.Firstname, OrderDTO.BillingAddress.Lastname, OrderDTO.BillingAddress.EmailAdress, OrderDTO.BillingAddress.AddressLine, OrderDTO.BillingAddress.Country, OrderDTO.BillingAddress.State, OrderDTO.BillingAddress.ZipCode);

            // Create the payment information from the OrdersDTO
            var payment = Payment.Of(OrderDTO.Payment.CardName, OrderDTO.Payment.CardNumber, OrderDTO.Payment.Expiration, OrderDTO.Payment.CVV, OrderDTO.Payment.PaymentMethod);

            // Create a new order instance
            var newOrder = Orders.Create(
                id: OrderId.Of(Guid.NewGuid()),
                cutomerID: CustomerId.Of(OrderDTO.CustomerId),
                orderName: OrderName.Of(OrderDTO.OrderName),
                shippingAdress: shippingAddress,
                billingAdress: billingAddress,
                payment: payment
            );

            // Add order items to the new order
            foreach (var orderItem in OrderDTO.OrderItems)
            {
                newOrder.Add(ProductId.Of(orderItem.ProductId), orderItem.Quantity, orderItem.Price);
            }

            // Return the new order
            return newOrder;
        }
    }
}
