namespace Basket.API.Baskets.CheckOutBasket;

public record CheckOutBasketRequest(BssketCheckOutDto BasketCheckOutDto);
public record CheckOutBasketResponse(bool isSuccess);
public class CheckOutBasketEndpoint : ICarterModule
{
     public void AddRoutes(IEndpointRouteBuilder app)
     {
          app.MapPost("/basket/checkout", async (CheckOutBasketRequest request, ISender sender) =>
          {

               var command = request.Adapt<CheckOutBasketCommand>();
               var result = await sender.Send(command);
               var response = result.Adapt<CheckOutBasketResponse>();
               return Results.Ok(response);
          }).RequireCustomAuth()
              .WithName("CheckOutbasket")
              .Produces<CheckOutBasketResponse>(StatusCodes.Status201Created)
              .ProducesProblem(StatusCodes.Status400BadRequest)
              .WithDescription("CheckOut basket service")
              ;
     }
}
