

namespace Basket.API.Baskets.UpdateBasket
{
     public record UpdateBasketRequest(ShoppingCart Cart) : ICommand<UpdateBasketResult>;
     public record UpdateBasketResponse(bool isSuccess);

     public class UpdateBasketEndpoint : ICarterModule
     {
          public void AddRoutes(IEndpointRouteBuilder app)
          {
               app.MapPut("/basket", async (UpdateBasketRequest request, ISender sender) =>
               {
                    var command = new UpdateBasketCommand(request.Cart);
                    var result = await sender.Send(command);
                    var response = new UpdateBasketResponse(result.isSuccess);

                    return Results.Ok(response);
               })
                  .RequireCustomAuth()
                   .WithName("UpdateBasket")
                   .Produces<UpdateBasketResponse>(200)
                   .ProducesProblem(StatusCodes.Status400BadRequest)
                   .ProducesProblem(StatusCodes.Status401Unauthorized)
                   .WithDescription("Updates an existing shopping cart for a user.")
                   .WithSummary("Updates an existing shopping cart for a user.");
          }
     }
}
