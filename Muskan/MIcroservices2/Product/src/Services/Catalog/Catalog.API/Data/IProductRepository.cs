namespace Catalog.API.Data
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken = default);
        Task<Product> UpdateProductAsync(Product product, CancellationToken cancellationToken = default);
        Task<bool> DeleteProductAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetProductsByPaginationAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category, CancellationToken cancellationToken);
    }
}

