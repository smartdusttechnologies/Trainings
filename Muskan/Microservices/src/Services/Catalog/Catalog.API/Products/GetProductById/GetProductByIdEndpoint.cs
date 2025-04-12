namespace Catalog.API.Products. GetProductById
{
    public record  GetProductByIdResponse(Product Products);
    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id}", async (Guid id ,ISender sender) =>
            {                
                                
                    var result = await sender.Send(new GetProductByIdQuery(id));


                var response = result.Adapt<GetProductByIdResponse>();
                // Return HTTP 201  Getd with response
               return Results.Ok(response);
              
                 
            

            }).WithName(" GetProductById")
            .Produces< GetProductByIdResponse>(StatusCodes.Status201Created).
            ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary(" Get Product")
            .WithDescription(" Gets a new product in the catalog.");
        }
    }
}
