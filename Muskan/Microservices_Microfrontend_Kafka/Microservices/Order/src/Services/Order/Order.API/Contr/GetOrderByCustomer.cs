//using Ordering.Application.Order.Queries.GetOrderByCustomer;
//using Ordering.Application.Services;

//namespace Ordering.API.Endpoints
//{

//     //public record GetOrderByCaustomerResponseRequest(string Customer);
//     public record GetOrderByCaustomerResponse(IEnumerable<OrdersDTO> orders);


//     public class GetOrderByCustomer : ICarterModule
//     {
//          public void AddRoutes(IEndpointRouteBuilder app)
//          {
//               app.MapGet("/orders/customer/{customerID}", async (Guid customerID, ISender sender, ILoggingService<GetOrderByCustomer> logger) =>
//               {
//                    TypeAdapterConfig<GetOrderByCustomerResult, GetOrderByCaustomerResponse>
//     .NewConfig()
//     .Map(dest => dest.orders, src => src.order);
//                    await logger.LogInformationAsync($"Get Order by Customer Id is called : {customerID}");
//                    var result = await sender.Send(new GetOrderByCustomerQuery(customerID));
//                    var response = result.Adapt<GetOrderByCaustomerResponse>();
//                    return Results.Ok(response);

//               }).RequireCustomAuth()
//                   .WithName("GetOrderByCustomer")
//                   .Produces<GetOrderByCaustomerResponse>(StatusCodes.Status200OK)
//                   .ProducesProblem(StatusCodes.Status400BadRequest)
//                   .ProducesProblem(StatusCodes.Status404NotFound)
//                   .WithDescription("Get Order By Customer")
//                   .WithSummary("Get Order By Customer");
//          }
//     }
//}
