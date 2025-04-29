namespace Ordering.Application.DTOs.Get
{
    public record GetOrderByCustomerResponse
    {
        public IEnumerable<OrdersDTO> Orders { get; set; }

        // Parameterless constructor
        public GetOrderByCustomerResponse() { }

        // Constructor to initialize the Orders property
        public GetOrderByCustomerResponse(IEnumerable<OrdersDTO> orders)
        {
            Orders = orders;
        }
    }
}
