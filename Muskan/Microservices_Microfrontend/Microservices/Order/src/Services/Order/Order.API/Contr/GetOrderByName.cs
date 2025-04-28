//using Ordering.Application.Order.Queries.GetOrderByName;
//using Ordering.Application.Services;
//namespace Ordering.API.Endpoints
//{
//     //public record GetOrderByNameRequest(string Name);
//     public record GetOrderByNameResponse(IEnumerable<OrdersDTO> orders);


//     public class GetOrderByName : ICarterModule
//     {
//          public void AddRoutes(IEndpointRouteBuilder app)
//          {
//               app.MapGet("/orders/{orderName}", async (string orderName, ISender sender, ILoggingService<GetOrderByName> logger) =>
//               {
//                    await logger.LogInformationAsync($"GetOrderByname is called: {orderName}");
//                    var result = await sender.Send(new GetOrderByNameQuery(orderName));
//                    var response = result.Adapt<GetOrderByNameResponse>();
//                    return Results.Ok(response);

//               }).RequireCustomAuth()
//                   .WithName("GetOrderByName")
//                   .Produces<GetOrderByNameResponse>(StatusCodes.Status200OK)
//                   .ProducesProblem(StatusCodes.Status400BadRequest)
//                   .ProducesProblem(StatusCodes.Status404NotFound)
//                   .WithDescription("Get Order By Name")
//                   .WithSummary("Get Order By Name");
//          }
//     }
//}
