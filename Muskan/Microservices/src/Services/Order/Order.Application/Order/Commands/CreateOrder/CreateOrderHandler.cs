namespace Ordering.Application.Order.Commands.CreateOrder
{
    public class CreateOrderHandler(IApplicationDbContext context) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
    {
        public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            //Create Order Entity from Command Object 
            //Save to Db
            // Return Results
            var order = CreateNewOrder(command.Order);

            context.Orders.Add(order);
            await context.SaveChangesAsync(cancellationToken);
           return new CreateOrderResult(order.Id.Value);
        }
        private Orders CreateNewOrder(OrdersDTO OrderDTO)
        {
            var shippingAddress = Address.Of(OrderDTO.ShippingAddress.Firstname, OrderDTO.ShippingAddress.Lastname, OrderDTO.ShippingAddress.EmailAdress, OrderDTO.ShippingAddress.AddressLine, OrderDTO.ShippingAddress.Country, OrderDTO.ShippingAddress.State, OrderDTO.ShippingAddress.ZipCode);

            var billingAddress = Address.Of(OrderDTO.BillingAddress.Firstname, OrderDTO.BillingAddress.Lastname, OrderDTO.BillingAddress.EmailAdress, OrderDTO.BillingAddress.AddressLine, OrderDTO.BillingAddress.Country, OrderDTO.BillingAddress.State, OrderDTO.BillingAddress.ZipCode);

            var newOrder = Orders.Create(
                id: OrderId.Of(Guid.NewGuid()),
                cutomerID: CustomerId.Of(OrderDTO.CustomerId),
                orderName: OrderName.Of(OrderDTO.OrderName),
                shippingAdress: shippingAddress,
                billingAdress: billingAddress,
                payment: Payment.Of(OrderDTO.payment.CardName, OrderDTO.payment.CardNumber, OrderDTO.payment.Expiration, OrderDTO.payment.cvv, OrderDTO.payment.PaymentMethod)
               );
            foreach (var orderItem in OrderDTO.OrderItems)
            {
                newOrder.Add(ProductId.Of(orderItem.ProductId), orderItem.Quantity, orderItem.Price);

            }
            return newOrder;

        } 
    }
}
