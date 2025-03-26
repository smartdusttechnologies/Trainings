namespace Catalog.API.Products. GetProduct
{
    public record  GetProductQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductResult>;
    public record  GetProductResult(IEnumerable<Product> Products);
    internal class GetProductQueryHandler(IProductRepository _productRepository, ILogger<GetProductQueryHandler> logger) : IQueryHandler<GetProductQuery,  GetProductResult>
    {
        public async Task< GetProductResult> Handle(GetProductQuery query , CancellationToken cancellationToken)
        
        {
            var products = await _productRepository.GetProductsByPaginationAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);
          

            // Return the product iddocker-compose up --build
            return new GetProductResult(products);

        }


    }
}
