namespace Ordering.Domain.Models
{
    /// <summary>
    /// Represents an order Entity.
    /// Represents an aggregate order containing multiple order items.
    /// </summary>
    public class Orders : Aggregate<OrderId>
    {
     /// <summary>
     /// OrderItem list
     /// </summary>
        private readonly List<OrderItem> _orderItems = new();
        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();
        /// <summary>
        /// CustomerID  
        /// </summary>
        public CustomerId CustomerId { get; private set; } = default;
        /// <summary>
        /// 
        /// </summary>
        public OrderName OrderName { get; private set; } = default;
        public Address ShippingAdress { get; private set; } = default;
        public Address Billingadress { get; private set; } = default;
        public Payment Payment { get; private set; } = default;
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;
        /// <summary>
        /// Calculates the total price based on the order items.
        /// </summary>
        public decimal TotalPrice {
            get => _orderItems.Sum(x => x.Price * x.Quantity);
                  private set { } 
        }
        /// <summary>
        /// Factory method to create a new order instance.
        /// </summary>
        public static Orders Create(OrderId id, CustomerId cutomerID, OrderName orderName , Address shippingAdress , Address billingAdress ,  Payment payment) {
            //ArgumentNullException.ThrowIfNull(orderId);
            //ArgumentNullException.ThrowIfNull(cutomerID);
            //ArgumentNullException.ThrowIfNull(orderName);

            var order = new Orders
            {
                Id = id,
                CustomerId = cutomerID,
                OrderName = orderName,
                ShippingAdress = shippingAdress,
                Billingadress = billingAdress,
                Payment = payment,
                Status = OrderStatus.Pending,


            };
            order.AddDomainEvent(new OrderCreatedEvent(order)); // Event-driven approach for order creation
            return order;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderName"></param>
        /// <param name="shippingAdress"></param>
        /// <param name="billingAdress"></param>
        /// <param name="payment"></param>
        /// <param name="status"></param>
        public void Update(OrderName orderName, Address shippingAdress, Address billingAdress, Payment payment ,OrderStatus status)
        {
            //ArgumentNullException.ThrowIfNull(orderName);
            //ArgumentNullException.ThrowIfNull(shippingAdress);
            //ArgumentNullException.ThrowIfNull(billingAdress);
            //ArgumentNullException.ThrowIfNull(payment);
            OrderName = orderName;
            ShippingAdress = shippingAdress;
            Billingadress = billingAdress;
            Payment = payment;
            Status = status;
            AddDomainEvent(new OrderUpdatedEvent(this)); // Event-driven approach for order update
        }
        /// <summary>
        /// Adds an order item to the order.
        /// </summary>
        public void Add(ProductId productId,int quantity, decimal price)
        {
     
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
            var orderItem = new OrderItem(Id, productId, quantity, price);
            _orderItems.Add(orderItem);
        }
        /// <summary>
        /// Removes an order item by its product ID.
        /// </summary>
        public void Remove(ProductId productId)
        {
            var orderItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);
            if (orderItem != null)
            {
                _orderItems.Remove(orderItem);
            }
        }

    }
}
