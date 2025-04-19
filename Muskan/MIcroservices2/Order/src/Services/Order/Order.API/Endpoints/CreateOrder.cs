
using Ordering.Application.Order.Commands.CreateOrder;
using Ordering.Application.Services;

namespace Ordering.API.Endpoints;
/// <summary>
/// Crete Order Request from Client
/// </summary>
/// <param name="Order"></param>
public record CreateOrderRequest(OrdersDTO Order);
/// <summary>
/// Create Order Response from server
/// </summary>
/// <param name="Id"></param>
public record CreateOrderResponse(Guid Id);
/// <summary>
/// 
/// </summary>
public class CreateOrder : ICarterModule
{
     /// <summary>
     /// Add Endpoint for Create Order
     /// </summary>
     /// <param name="app"></param>
     public void AddRoutes(IEndpointRouteBuilder app)
     {

          app.MapPost("/orders", async (CreateOrderRequest request, ISender sender, ILoggingService<CreateOrder> logger) =>
          {
               //Map the CreateOrderRequest and CreateOrderCommand
               TypeAdapterConfig<CreateOrderRequest, CreateOrderCommand>
     .NewConfig()
     .Map(dest => dest.Order.Payment.CVV, src => src.Order.Payment.CVV);
               await logger.LogInformationAsync($"Create Order Endpoint hit : {request}");
               //request map to the the command 
               var command = request.Adapt<CreateOrderCommand>();
               //Command send and then to handler
               var result = await sender.Send(command);
               //response get from the command and map to the CreateOrderResponse
               var response = result.Adapt<CreateOrderResponse>();
               //Return the results
               return Results.Created($"/orders/{response.Id}", response);
          })
                 .RequireCustomAuth()
        //Assigns a name "CreateOrder" to the endpoint, making it easier to reference in tools like OpenAPI(Swagger).
        .WithName("CreateOrder")
        //Specifies that the API returns a 201 Created status code with a response of type CreateOrderResponse.
        .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
        //Specifies that the API can return a 400 Bad Request response in case of errors, following the Problem Details format.
        .ProducesProblem(StatusCodes.Status400BadRequest)
        //Provides a short summary for the API endpoint, useful for documentation(e.g., Swagger UI).
        .WithSummary("Create Order")
        //Provides a detailed description of the API endpoint.
        .WithDescription("Create Order");
     }
}
