namespace Basket.API.Baskets.StoreBasket
{
    public record StoreBasketRequest(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResponse(string userName);

    public class StoreBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (StoreBasketRequest request , ISender sender) =>
            {  
                var command = new StoreBasketCommand(request.Cart);
                var result = await sender.Send(command);
                var response = new StoreBasketResponse(result.userName);

                return Results.Created($"/basket/{response.userName}",response);

            })
                .WithName("StoreBasket")
                .Produces<StoreBasketResponse>(201)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithDescription("Stores a shopping cart for a user.")
                .WithSummary("Stores a shopping cart for a user.");
        }
    }
}
