namespace Basket.API.Baskets.GetBasket
{
     //public record GetBasketRequest(string Username);
     public record GetBasketResponse(ShoppingCart Cart);
     public class GetBasketEndpoint : ICarterModule

     {
          public void AddRoutes(IEndpointRouteBuilder app)
          {
               app.MapGet("/basket/{username}", async (string userName, ISender sender, ILoggingService<GetBasketEndpoint> logger) =>
               {
                    try
                    {
                         await logger.LogInformationAsync("Getting basket for user: " + userName);
                         var result = await sender.Send(new GetBasketQuery(userName));

                         var response = result.Adapt<GetBasketResponse>();

                         return Results.Ok(response);
                    }
                    catch
                    {
                         await logger.LogErrorAsync("Error getting basket for user: " + userName, new Exception("Basket not found"));
                         return Results.Problem("An error occurred while getting the basket.", statusCode: StatusCodes.Status500InternalServerError);
                    }


               }).RequireCustomAuth()
                   .WithName("GetBasket")
                   .Produces<GetBasketResponse>(200)
               .ProducesProblem(404)
               .WithSummary("Get a user's shopping cart")
               .WithDescription("Retrieves a user's shopping cart by username");


          }
     }
}
