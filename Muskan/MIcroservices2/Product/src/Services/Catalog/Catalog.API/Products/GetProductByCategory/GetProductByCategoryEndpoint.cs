namespace Catalog.API.Products.GetProductByCategory
{
     public record GetProductByCategoryResponse(IEnumerable<Product> Products);
     public class GetProductByCategoryEndpoint : ICarterModule
     {
          public void AddRoutes(IEndpointRouteBuilder app)
          {
               app.MapGet("/products/category/{category}", async (string category, ISender sender, ILoggingService<GetProductByCategoryEndpoint> logger) =>
               {
                    try
                    {
                         await logger.LogInformationAsync("Getting a product by category...");
                         var result = await sender.Send(new GetProductByCategoryQuery(category));
                         if (result == null)
                         {

                              return Results.NotFound("Product not found");
                         }
                         var response = result.Adapt<GetProductByCategoryResponse>();

                         // Return HTTP 201  Getd with response
                         return Results.Ok(response);
                    }
                    catch (Exception ex)
                    {
                         await logger.LogErrorAsync("Error occured in the GetProductByCategory endpoint", ex);
                         return Results.BadRequest(ex);
                    }





               }).WithName(" GetProductByCategory")
               .Produces<GetProductByCategoryResponse>(StatusCodes.Status201Created).
               ProducesProblem(StatusCodes.Status400BadRequest)
               .WithSummary(" Get Product")
               .WithDescription(" Gets a new product with category in the catalog.");
          }
     }
}
