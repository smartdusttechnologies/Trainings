namespace Catalog.API.Data
{
    public class ProductRepository(ApplicationDbContext _context) : IProductRepository
    {
        // Get all products
        public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Products.ToListAsync(cancellationToken);
        }

        // Get a product by ID
        public async Task<Product> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        // Add a new product
        public async Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken = default)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);
            return product;
        }

        // Update an existing product
        public async Task<Product> UpdateProductAsync(Product product, CancellationToken cancellationToken = default)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync(cancellationToken);
            return product;
        }

        // Delete a product by ID
        public async Task<bool> DeleteProductAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async  Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category, CancellationToken cancellationToken)
        {
            var products =  await _context.Products
               .Where(p => p.Category.Contains(category))              
               .ToListAsync(cancellationToken);
            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByPaginationAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {          
           
            var totalCount = await _context.Products.LongCountAsync(cancellationToken);
   
            var products = await _context.Products                 
                .OrderBy(o => o.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
            return products;
        }     
    }
}
