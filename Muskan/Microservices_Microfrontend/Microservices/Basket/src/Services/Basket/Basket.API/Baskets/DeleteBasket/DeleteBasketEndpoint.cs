
//using Basket.API.Baskets.StoreBasket;

//namespace Basket.API.Baskets.DeleteBasket
//{
//     public record DeleteBasketRequest(string UserName) : ICommand<DeleteBasketResult>;
//     public record DeleteBasketResponse(bool isSuccess);

//     public class DeleteBasketEndpoint : ICarterModule
//     {
//          public void AddRoutes(IEndpointRouteBuilder app)
//          {
//               app.MapDelete("/basket/{userName}", async (string userName, ISender sender, ILoggingService<DeleteBasketEndpoint> logger) =>
//               {
//                    try
//                    {
//                         await logger.LogInformationAsync("Deleting basket for user: " + userName);
//                         var result = await sender.Send(new DeleteBasketCommand(userName));
//                         var response = result.Adapt<DeleteBasketResponse>();
//                         return Results.Ok(response);
//                    }
//                    catch (Exception ex)
//                    {
//                         await logger.LogErrorAsync("Error deleting basket for user: " + userName, ex);
//                         return Results.Problem("An error occurred while deleting the basket.", statusCode: StatusCodes.Status500InternalServerError);
//                    }



//               }).RequireCustomAuth()
//                   .WithName("deleteBAsket")
//                   .Produces<StoreBasketResponse>(201)
//                   .ProducesProblem(StatusCodes.Status400BadRequest)
//                   .WithDescription("Delete a shopping cart for a user.")
//                   .WithSummary("Delete a shopping cart for a user.");

//          }
//     }

//}
