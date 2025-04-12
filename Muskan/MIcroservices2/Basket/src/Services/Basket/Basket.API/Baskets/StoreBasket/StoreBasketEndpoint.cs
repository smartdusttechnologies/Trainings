namespace Basket.API.Baskets.StoreBasket
{
     public record StoreBasketRequest(ShoppingCart Cart) : ICommand<StoreBasketResult>;
     public record StoreBasketResponse(string userName);

     public class StoreBasketEndpoint : ICarterModule
     {
          private readonly HttpClient _httpClient;

          public StoreBasketEndpoint(IHttpClientFactory httpClientFactory)
          {
               _httpClient = httpClientFactory.CreateClient("CommonService");
          }
          public void AddRoutes(IEndpointRouteBuilder app)
          {
               app.MapPost("/basket", async (StoreBasketRequest request, ISender sender, HttpContext httpContext, ILoggingService<StoreBasketEndpoint> logger) =>
               {
                    try
                    {
                         await logger.LogInformationAsync("Storing basket for user: " + request.Cart.UserName);
                         var command = new StoreBasketCommand(request.Cart);
                         var result = await sender.Send(command);
                         var response = new StoreBasketResponse(result.userName);
                         return Results.Created($"/basket/{response.userName}", response);

                    }
                    catch (Exception ex)
                    {
                         await logger.LogErrorAsync("Error storing basket", ex);
                         return Results.Problem("An error occurred while storing the basket.", statusCode: StatusCodes.Status500InternalServerError);
                    }

               })
                     .RequireCustomAuth()
                   .WithName("StoreBasket")
                   .Produces<StoreBasketResponse>(201)
                   .ProducesProblem(StatusCodes.Status400BadRequest)
                   .WithDescription("Stores a shopping cart for a user.")
                   .WithSummary("Stores a shopping cart for a user.");
          }
     }
}
