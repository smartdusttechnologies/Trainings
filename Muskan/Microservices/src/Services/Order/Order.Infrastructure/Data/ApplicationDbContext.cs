using Ordering.Application.Data;

namespace Ordering.Infrastructure.Data
{
    /// <summary>
    /// DbContext for Order
    /// </summary>
    public class ApplicationDbContext : DbContext ,IApplicationDbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="option">The options to be used by a <see cref="DbContext"/>.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option ) : base(option)
        {
            
        }

        /// <summary>
        /// Gets or sets the Customers DbSet.
        /// </summary>
        public DbSet<Customer> Customers => Set<Customer>();

        /// <summary>
        /// Gets or sets the Orders DbSet.
        /// </summary>
        public DbSet<Orders> Orders => Set<Orders>();

        /// <summary>
        /// Gets or sets the OrderItems DbSet.
        /// </summary>
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        /// <summary>
        /// Gets or sets the Products DbSet.
        /// </summary>
        public DbSet<Product> Products => Set<Product>();

        /// <summary>
        /// Configures the model that was discovered by convention from the entity types
        /// exposed in <see cref="DbSet{TEntity}"/> properties on your derived context.
        /// </summary>
        /// <param name="builder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
