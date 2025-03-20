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
        /// <param name="option"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option ) : base(option)
        {
            
        }
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Orders> Orders => Set<Orders>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Product> Products => Set<Product>();
   

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
