using Catalog.API.Exceptions;

namespace Catalog.API.Products. GetProductByCategory
{
    public record GetProductByCategoryQuery(string Category ) : IQuery<GetProductByCategoryResult>;
    public record GetProductByCategoryResult(IEnumerable<Product> Products);
    internal class GetProductByCategoryQueryHandler(IProductRepository repository, ILogger<GetProductByCategoryQueryHandler> logger) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductByCategoryQuery.Handle", query);

           var product = await repository.GetProductsByCategoryAsync(query.Category, cancellationToken);
            return new GetProductByCategoryResult(product);

                }
    }
}
