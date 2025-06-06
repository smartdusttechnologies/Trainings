﻿using Catalog.API.Exceptions;

namespace Catalog.API.Products.GetProductById
{
     public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
     public record GetProductByIdResult(Product Products);
     internal class GetProductByIdQueryHandler(IProductRepository repository, ILoggingService<GetProductByIdQueryHandler> logger) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
     {
          public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
          {
               await logger.LogInformationAsync($"GetProductByIdQueryHandler.Handle , {query}");

               var product = await repository.GetProductByIdAsync(query.Id, cancellationToken);
               if (product == null)
               {
                    throw new ProductNotFoundException(query.Id);
               }

               return new GetProductByIdResult(product);
          }
     }
}
