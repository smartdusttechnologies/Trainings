namespace Basket.API.Baskets.GetBasket
{
    //public record GetBasketRequest(string Username);
    public record GetBasketResponse(ShoppingCart Cart);
    public class GetBasketEndpoint : ICarterModule

    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{username}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new GetBasketQuery(userName));

                var response = result.Adapt<GetBasketResponse>();

                return Results.Ok(response);

            })
                .WithName("GetBasket")
                .Produces<GetBasketResponse>(200)
            .ProducesProblem(404)
            .WithSummary("Get a user's shopping cart")
            .WithDescription("Retrieves a user's shopping cart by username");


        }
    }
}
