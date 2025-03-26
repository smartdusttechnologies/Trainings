namespace Shopping.Web.Services
{
    /// <summary>
    /// Defines operations related to ordering services.
    /// This interface is used to interact with an ordering service API.
    /// </summary>
    public interface IOrderingService
    {
        [Get("/ordering-service/orders?pageIndex={pageIndex}&pageSize={pageSize}\"")]
        Task<GetOrdersResponse> GetOrders(int? pageIndex = 1, int? pageSize = 10);

        [Get("/ordering-service/orders/{orderName}")]
        Task<GetOrdersByNameResponse> GetOrdersByName(string orderName);

        [Get("/ordering-service/orders/customer/{customerId}")]
        Task<GetOrdersByCustomerResponse> GetOrdersByCustomer(Guid customerId);
    }

}
