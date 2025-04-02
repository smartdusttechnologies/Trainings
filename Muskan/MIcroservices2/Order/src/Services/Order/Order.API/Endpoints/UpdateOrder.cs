using Ordering.Application.Order.Commands.CreateOrder;
using Ordering.Application.Order.Commands.UpdateOrder;

namespace Ordering.API.Endpoints
{ 
    public record UpdateOrderRequest(OrdersDTO order);
    public record UpdateOrderResponse(bool isSuccess);

    public class UpdateOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/orders", async (UpdateOrderRequest request, ISender sender) =>
            {
                TypeAdapterConfig<UpdateOrderResult, UpdateOrderResponse>
    .NewConfig()
    .Map(dest => dest.isSuccess, src => src.isSuccess);

                //var command = request.Adapt<UpdateOrderCommand>();
                var command = new UpdateOrderCommand(request.order);

               

                var result = await sender.Send(command);

                Console.WriteLine($"Result from sender.Send: {result.isSuccess}");

                var response = result.Adapt<UpdateOrderResponse>();

                Console.WriteLine($"Mapped response: {response.isSuccess}");


                return Results.Ok(response);
            })
                .WithName("UpdateOrder")
                .Produces<UpdateOrderResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithDescription("Update Order")
                .WithSummary("Update Order")
                ;
        }
    }
}
