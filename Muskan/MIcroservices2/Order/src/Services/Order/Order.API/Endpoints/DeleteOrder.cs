using Ordering.Application.Order.Commands.DeleteOrder;
using Ordering.Application.Services;

namespace Ordering.API.Endpoints
{
     //public record DeleteOrderRequest(Guid Id);
     /// <summary>
     /// DeleteOrderResponse response from the server
     /// </summary>
     /// <param name="isSuccess"></param>
     public record DeleteOrderResponse(bool isSuccess);

     /// <summary>
     /// Class that handles routing of the Delete Order
     /// </summary>
     public class DeleteOrder : ICarterModule
     {
          /// <summary>
          /// Adds the routes for the Delete Order endpoint
          /// </summary>
          /// <param name="app">The endpoint route builder</param>
          public void AddRoutes(IEndpointRouteBuilder app)
          {
               /// <summary>
               /// Maps the DELETE request to the DeleteOrderCommand
               /// </summary>
               app.MapDelete("/orders/{id}", async (Guid id, ISender sender, ILoggingService<DeleteOrder> logger) =>
               {
                    // Map between the DeleteOrderResult and DeleteOrderResponse
                    TypeAdapterConfig<DeleteOrderResult, DeleteOrderResponse>
                     .NewConfig()
                     .Map(dest => dest.isSuccess, src => src.isSuccess);
                    await logger.LogInformationAsync($"Delete Order Endpoint hit : {id}");
                    // Send the DeleteOrderCommand
                    var result = await sender.Send(new DeleteOrderCommand(id));
                    // Adapt the result to the response
                    var response = result.Adapt<DeleteOrderResponse>();
                    return Results.Ok(response);
               })
                       .RequireCustomAuth()
               .WithName("DeleteOrder")
               .Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
               .ProducesProblem(StatusCodes.Status400BadRequest)
               .WithDescription("Delete Order Order")
               .WithSummary("Delete Order");
          }
     }
}