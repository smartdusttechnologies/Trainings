
namespace Catalog.API.Products. GetProduct
{
    public record  GetProductRequest(int? PageNumber = 1 , int? PageSize = 10);
    public record  GetProductResponse(IEnumerable<Product> Products);
    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async ([AsParameters] GetProductRequest request , ISender sender) =>
            {              var query = request.Adapt<GetProductQuery>();

                var result = await sender.Send(query);


                var response = result.Adapt< GetProductResponse>();
                // Return HTTP 201  Getd with response
               return Results.Ok(response);
             
            
      
             

            }).WithName(" GetProduct")
            .Produces< GetProductResponse>(StatusCodes.Status201Created).
            ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary(" Get Product")
            .WithDescription(" Gets a new product in the catalog.");
        }

        private RequestDelegate async(object value, GetProductRequest getProductRequest, object request, ISender sender1, object sender2)
        {
            throw new NotImplementedException();
        }
    }
}
