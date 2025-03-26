using Microsoft.EntityFrameworkCore;

namespace Basket.API.Data
{
    public class ApplicationDbContext  : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-many relationship between ShoppingCart and ShoppingCartItem
            modelBuilder.Entity<ShoppingCart>()
                .HasMany(sc => sc.Items)  // A ShoppingCart has many ShoppingCartItems
                .WithOne(item => item.ShoppingCart)  // Each ShoppingCartItem has one ShoppingCart
                .HasForeignKey(item => item.ShoppingCartId);  // The foreign key is ShoppingCartId
        }
    }
}
