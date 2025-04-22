namespace Catalog.API.Data
{
     public class ProductRepository(ApplicationDbContext context,ILoggingService<ProductRepository> logger ): IProductRepository
     {          

          public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken = default)
          {
               await logger.LogInformationAsync("Fetching all products.");
               return await context.Products.ToListAsync(cancellationToken);
          }

          public async Task<Product> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default)
          {
               await logger.LogInformationAsync($"Fetching product with ID: {id}");
               return await context.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
          }

          public async Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken = default)
          {
               try
               {
                    context.Products.Add(product);
                    await context.SaveChangesAsync(cancellationToken);
                    await logger.LogInformationAsync($"Product added successfully. ID: {product.Id}");
                    return product;
               }
               catch (Exception ex)
               {
                    await logger.LogErrorAsync("Error adding product.", ex);
                    throw;
               }
          }

          public async Task<Product> UpdateProductAsync(Product product, CancellationToken cancellationToken = default)
          {
               try
               {
                    context.Products.Update(product);
                    await context.SaveChangesAsync(cancellationToken);
                    await logger.LogInformationAsync($"Product updated successfully. ID: {product.Id}");
                    return product;
               }
               catch (Exception ex)
               {
                    await logger.LogErrorAsync("Error updating product.", ex);
                    throw;
               }
          }

          public async Task<bool> DeleteProductAsync(Guid id, CancellationToken cancellationToken = default)
          {
               try
               {
                    var product = await context.Products.FindAsync(id);
                    if (product == null)
                    {
                         await logger.LogWarningAsync($"Product not found for deletion. ID: {id}");
                         return false;
                    }

                    context.Products.Remove(product);
                    await context.SaveChangesAsync(cancellationToken);
                    await logger.LogInformationAsync($"Product deleted successfully. ID: {id}");
                    return true;
               }
               catch (Exception ex)
               {
                    await logger.LogErrorAsync($"Error deleting product with ID: {id}", ex);
                    throw;
               }
          }

          public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category, CancellationToken cancellationToken)
          {
               await logger.LogInformationAsync($"Fetching products for category: {category}");
               return await context.Products
                   .Where(p => p.Category.Contains(category))
                   .ToListAsync(cancellationToken);
          }

          public async Task<IEnumerable<Product>> GetProductsByPaginationAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
          {
               await logger.LogInformationAsync($"Fetching paginated products. Page: {pageNumber}, Size: {pageSize}");

               var totalCount = await context.Products.LongCountAsync(cancellationToken);
               var products = await context.Products
                   .OrderBy(o => o.Name)
                   .Skip((pageNumber - 1) * pageSize)
                   .Take(pageSize)
                   .ToListAsync(cancellationToken);

               await logger.LogDebugAsync($"Retrieved {products.Count} products out of {totalCount} total.");
               return products;
          }
     }
}
