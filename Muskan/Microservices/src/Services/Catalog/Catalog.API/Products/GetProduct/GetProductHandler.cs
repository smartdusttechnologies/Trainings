namespace Catalog.API.Products. GetProduct
{
    public record  GetProductQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductResult>;
    public record  GetProductResult(IEnumerable<Product> Products);
    internal class GetProductQueryHandler(IDocumentSession session ,ILogger<GetProductQueryHandler> logger) : IQueryHandler<GetProductQuery,  GetProductResult>
    {
        public async Task< GetProductResult> Handle(GetProductQuery query , CancellationToken cancellationToken)
        
        {       
           var product = await session.Query<Product>().ToPagedListAsync(query.PageNumber ?? 1 , query.PageSize ?? 10, cancellationToken);
            // Assuming SaveProductAsync is a method that saves the product and returns the product ID


            // Return the product iddocker-compose up --build

            return new  GetProductResult(product);
        }


    }
}
