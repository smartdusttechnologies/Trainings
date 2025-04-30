namespace Ordering.Application.Order.Commands.UpdateOrder
{
     // Handler for the UpdateOrderCommand
     public class UpdateOrderHandler(IApplicationDbContext context, ILoggingService<UpdateOrderHandler> logger) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
     {
          // Method to handle the UpdateOrderCommand
          public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
          {
               await logger.LogInformationAsync($"Update order handle called {command}");
               // Get the order ID from the command
               var orderId = OrderId.Of(command.Order.Id);

               // Find the order in the context
               var order = await context.Orders.FindAsync([orderId], cancellationToken);

               // If the order is not found, throw an exception
               if (order == null)
               {
                    throw new OrderNotFoundException(command.Order.Id);
               }

               // Update the order with new values from the command
               UpdateOrderWithNewValue(order, command.Order);

               // Update the order in the context
               context.Orders.Update(order);

               // Save changes to the database
               await context.SaveChangesAsync(cancellationToken);

               // Return the result indicating success
               return new UpdateOrderResult(true);
          }

          // Method to update the order with new values from the OrdersDTO
          public void UpdateOrderWithNewValue(Orders order, OrdersDTO orderDTO)
          {
               // Create the updated shipping address from the OrdersDTO
               var updateshippingAddress = Address.Of(orderDTO.ShippingAddress.Firstname, orderDTO.ShippingAddress.Lastname, orderDTO.ShippingAddress.EmailAdress, orderDTO.ShippingAddress.AddressLine, orderDTO.ShippingAddress.Country, orderDTO.ShippingAddress.State, orderDTO.ShippingAddress.ZipCode);

               // Create the updated billing address from the OrdersDTO
               var updatebillingAddress = Address.Of(orderDTO.BillingAddress.Firstname, orderDTO.BillingAddress.Lastname, orderDTO.BillingAddress.EmailAdress, orderDTO.BillingAddress.AddressLine, orderDTO.BillingAddress.Country, orderDTO.BillingAddress.State, orderDTO.BillingAddress.ZipCode);

               // Create the updated payment information from the OrdersDTO
               var updatePayment = Payment.Of(orderDTO.Payment.CardName, orderDTO.Payment.CardNumber, orderDTO.Payment.Expiration, orderDTO.Payment.CVV, orderDTO.Payment.PaymentMethod);

               // Update the order with the new values
               order.Update(
                   orderName: OrderName.Of(orderDTO.OrderName),
                   shippingAdress: updateshippingAddress,
                   billingAdress: updatebillingAddress,
                   payment: updatePayment,
                   status: orderDTO.Status
               );
          }
     }
}
