namespace Catalog.API.Products.GetProductByCategory
{
     public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
     public record GetProductByCategoryResult(IEnumerable<Product> Products);
     internal class GetProductByCategoryQueryHandler(IProductRepository repository, ILoggingService<GetProductByCategoryQueryHandler> logger) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
     {
          public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
          {
               try
               {
                    await logger.LogInformationAsync("Get the product by the category ");

                    var product = await repository.GetProductsByCategoryAsync(query.Category, cancellationToken);
                    return new GetProductByCategoryResult(product);
               }
               catch (Exception ex)
               {
                    await logger.LogErrorAsync("An error occurred while getting a product by category.", ex);
                    return new GetProductByCategoryResult(null);
               }
               finally
               {
                    await logger.LogInformationAsync("Exit from the GetProductByCategoryQueryHandler");
               }


          }
     }
}
