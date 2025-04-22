namespace Catalog.API.Products.UpdateProduct
{
     public record UpdateProductRequest(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price);
     public record UpdateProductResponse(bool IsSuccess);

     public class UpdateProductEndpoint : ICarterModule
     {
          public void AddRoutes(IEndpointRouteBuilder app)
          {
               app.MapPut("/products",
                   async (UpdateProductRequest request, ISender sender ,ILoggingService<UpdateProductEndpoint> logger) =>
                   {
                     await logger.LogInformationAsync($"Received update request for Product Id: {request.Id}");

                        var command = request.Adapt<UpdateProductCommand>();
  UpdateProductResult result;

                    try
                    {
                        result = await sender.Send(command);
                    }
                    catch (ProductNotFoundException ex)
                    {
                        await logger.LogErrorAsync($"Product not found for Id: {command.Id}", ex);
                        return Results.Problem(
                            detail: $"Product with ID {command.Id} was not found.",
                            statusCode: StatusCodes.Status404NotFound,
                            title: "Product Not Found");
                    }
                    catch (Exception ex)
                    {
                        await logger.LogErrorAsync("Unexpected error occurred while updating product", ex);
                        return Results.Problem(
                            detail: "An unexpected error occurred while updating the product.",
                            statusCode: StatusCodes.Status500InternalServerError,
                            title: "Internal Server Error");
                    }

                    if (!result.IsSuccess)
                    {
                        await logger.LogWarningAsync($"Update failed for Product Id: {command.Id}");
                        return Results.Problem(
                            detail: $"Failed to update Product with ID {command.Id}",
                            statusCode: StatusCodes.Status404NotFound,
                            title: "Update Failed");
                    }

                    await logger.LogInformationAsync($"Successfully updated Product Id: {command.Id}");
  
 
                        var response = result.Adapt<UpdateProductResponse>();

                        return Results.Ok(response);
                   })
                       .RequireCustomAuth()
                   .WithName("UpdateProduct")
                   .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
                   .ProducesProblem(StatusCodes.Status400BadRequest)
                   .ProducesProblem(StatusCodes.Status404NotFound)
                   .WithSummary("Update Product")
                   .WithDescription("Update Product");
          }
     }
}
