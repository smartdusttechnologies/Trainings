//using Ordering.Application.Order.Queries.GetOrders;
//using Ordering.Application.Services;

//namespace Ordering.API.Endpoints
//{
//     /// <summary>
//     ///  A record for handling GetOrder request 
//     /// </summary>
//     /// <param name="PaginateRequest"></param>
//     //public record GetOrderRequest(PaginateRequest PaginateRequest);
//     /// <summary>
//     /// Define a record to store paginated order results
//     /// </summary>
//     /// <param name="PaginateResult"></param>
//     public record GetOrderResponse(PaginateResult<OrdersDTO> PaginateResult);
//     /// <summary>
//     /// Get Order Endpoint 
//     /// Define a class implementing the ICarterModule interface for API routes
//     /// </summary>
//     public class GetOrder : ICarterModule
//     {
//          /// <summary>
//          /// Adds the API routes for fetching orders
//          /// </summary>
//          /// <param name="app">IEndpointRouteBuilder instance to define endpoints</param>
//          public void AddRoutes(IEndpointRouteBuilder app)
//          {
//               app.MapGet("/orders", async ([AsParameters] PaginateRequest request, ISender sender, ILoggingService<GetOrder> logger) =>
//               {
//                    // Configuring the type adapter to map GetOrderResult to GetOrderResponse
//                    // Define a TypeAdapterConfig mapping source to destination
//                    TypeAdapterConfig<GetOrderResult, GetOrderResponse>
//                     // Create a new configuration for mapping
//                     .NewConfig()
//                     // Map the paginated result from request to response
//                     .Map(dest => dest.PaginateResult, src => src.PaginateRequest);
//                    // Send a GetOrdersQuery request and await the result
//                    await logger.LogInformationAsync($"Get Order endpoint hit : {request}");
//                    var result = await sender.Send(new GetOrdersQuery(request));
//                    // Convert the result to GetOrderResponse format using adaptation

//                    var response = result.Adapt<GetOrderResponse>();

//                    return Results.Ok(response);

//               }).RequireCustomAuth()
//                   .WithName("GetOrder")
//                   .Produces<GetOrderResponse>(StatusCodes.Status200OK)
//                   .ProducesProblem(StatusCodes.Status400BadRequest)
//                   .ProducesProblem(StatusCodes.Status404NotFound)
//                   .WithDescription("Get Order")
//                   .WithSummary("Get Order ");
//          }
//     }
//}
