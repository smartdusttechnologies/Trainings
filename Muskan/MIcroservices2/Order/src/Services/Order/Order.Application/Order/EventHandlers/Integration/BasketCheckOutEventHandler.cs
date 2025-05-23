﻿using BuildingBlock.Messaging.Events;
using MassTransit;
using Ordering.Application.Order.Commands.CreateOrder;

namespace Ordering.Application.Order.EventHandlers.Integration
{
     public class BasketCheckOutEventHandler(ISender sender, ILoggingService<BasketCheckOutEventHandler> logger) : IConsumer<BasketCheckOutEvents>
     {
          public async Task Consume(ConsumeContext<BasketCheckOutEvents> context)
          {
               //Create a new order and start order fullfillment process
               try
               {
                    await logger.LogInformationAsync($"Integration Event handled : {context.Message.GetType().Name}");
                    Console.WriteLine($"Received checkout event: {context.Message}");
                    var command = MapToCreateOrderCommand(context.Message);
                    await sender.Send(command);
                    await logger.LogInformationAsync("Order creation command sent successfully.");
               }
               catch (Exception ex)
               {
                    await logger.LogErrorAsync($"Error processing integration event: {context.Message.GetType().Name}", ex);
                    throw;
               }
          }
          private CreateOrderCommand MapToCreateOrderCommand(BasketCheckOutEvents message)
          {
               var addressDTO = new AddressDTO(message.FirstName, message.LastName, message.EmailAdress, message.AdressLine, message.Country, message.State, message.ZipCode);

               var paymentDto = new PaymentDTO(message.CardName, message.CardNumber, message.ExpiryDate, message.CVV, message.PaymentMethod);

               // Order Id 
               var orderId = Guid.NewGuid();

               var orderDto = new OrdersDTO(
                   Id: orderId,
                   CustomerId: message.CustomerId,
                   OrderName: message.Username,
                   ShippingAddress: addressDTO,
                   BillingAddress: addressDTO,
                   Payment: paymentDto,
                   Status: OrderStatus.Pending,
                   OrderItems:
                   [
                   new OrderItemDTO(orderId, new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"), 2, 500),
                        new OrderItemDTO(orderId, new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"), 1, 400)
                   ]
                   );
               return new CreateOrderCommand(orderDto);
          }
     }

}
