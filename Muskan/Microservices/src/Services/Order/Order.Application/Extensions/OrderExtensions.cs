

using Ordering.Application.DTOs;

namespace Ordering.Application.Extensions
{
    /// <summary>
    /// Contains extension methods for order-related operations
    /// </summary>
    public static class OrderExtensions
    {
        /// <summary>
        /// Converts a collection of Orders to a collection of OrdersDTO
        ///  Extension method to convert Orders to OrdersDTO
        /// </summary>
        /// <param name="orders">Collection of Orders to be converted</param>
        /// <returns>Collection of OrdersDTO</returns>
        public static IEnumerable<OrdersDTO> ToOrderDtoList(this IEnumerable<Orders> orders)
        {
            /// Converts each order in the collection to an OrdersDTO instance
            return orders.Select(
                order => new OrdersDTO(
                    Id: order.Id.Value,
                    CustomerId: order.CustomerId.Value,
                    OrderName: order.OrderName.Value,
                    ShippingAddress: new AddressDTO(
                        order.ShippingAdress.FirstName,
                        order.ShippingAdress.LastName,
                        order.ShippingAdress.EmailAdress,
                        order.ShippingAdress.AdressLine,
                        order.ShippingAdress.Country,
                        order.ShippingAdress.State,
                        order.ShippingAdress.ZipCode
                    ),
                    BillingAddress: new AddressDTO(
                        order.Billingadress.FirstName,
                        order.Billingadress.LastName,
                        order.Billingadress.EmailAdress,
                        order.Billingadress.AdressLine,
                        order.Billingadress.Country,
                        order.Billingadress.State,
                        order.Billingadress.ZipCode),

                    Payment: new PaymentDTO(
                        order.Payment.CardName,
                        order.Payment.CardNumber,
                        order.Payment.ExpiryDate,
                        order.Payment.CVV,
                        order.Payment.PaymentMethod
                        ),
                    Status: order.Status,
                    OrderItems: order.OrderItems.Select(oi => new OrderItemDTO(
                        oi.OrderId.Value,
                        oi.ProductId.Value,
                        oi.Quantity,
                        oi.Price
                        )).ToList()
                        ));

        }
        public static OrdersDTO ToOrderDTO(this Orders order)
        {
            return DtoFromOrder(order);
        }
        private static OrdersDTO DtoFromOrder(Orders order)
        {
            return new OrdersDTO(
                        Id: order.Id.Value,
                        CustomerId: order.CustomerId.Value,
                        OrderName: order.OrderName.Value,
                        ShippingAddress: new AddressDTO(order.ShippingAdress.FirstName, order.ShippingAdress.LastName, order.ShippingAdress.EmailAdress!, order.ShippingAdress.AdressLine, order.ShippingAdress.Country, order.ShippingAdress.State, order.ShippingAdress.ZipCode),
                        BillingAddress: new AddressDTO(order.Billingadress.FirstName, order.Billingadress.LastName, order.Billingadress.EmailAdress!, order.Billingadress.AdressLine, order.Billingadress.Country, order.Billingadress.State, order.Billingadress.ZipCode),
                        Payment: new PaymentDTO(order.Payment.CardName!, order.Payment.CardNumber, order.Payment.ExpiryDate, order.Payment.CVV, order.Payment.PaymentMethod),
                        Status: order.Status,
                        OrderItems: order.OrderItems.Select(oi => new OrderItemDTO(oi.OrderId.Value, oi.ProductId.Value, oi.Quantity, oi.Price)).ToList()
                    );
        }
    }
}
/// <summary>
/// 1.Why is this an Extension Method?
/// Extension methods allow you to add new methods to existing types without modifying their source code.
/// The ToOrderDtoList method extends IEnumerable<Orders>, enabling any IEnumerable<Orders> collection to call this method directly.
/// The this keyword before IEnumerable<Orders> orders in the method signature indicates that this is an extension method.
/// </summary>

/// <summary>
/// 2. Why Use static?
/// Static Class:
/// Extension methods must be defined inside a static class to prevent instantiation.
/// OrderExtensions is a static class because it only contains extension methods.
/// Static Method:
/// The method ToOrderDtoList must be static because extension methods require a static context.
/// A static method doesn't need an instance of the class to be used.
/// </summary>
/// <summary>
/// How to use this : ("orders.ToOrderDtoList(); ")
/// Example
/// IEnumerable<OrdersDTO> orderDtos = orders.ToOrderDtoList(); 
/// </summary>