using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


namespace Ordering.Infrastructure.Data.Extensions
{
    /// <summary>
    /// Provides extension methods for initializing and seeding the database.
    /// </summary>
    public static class DatabaseExtension
    {
        /// <summary>
        /// Ensures the database is up-to-date with pending migrations and seeds initial data.
        /// This method should be called during application startup.
        /// </summary>
        /// <param name="app">The WebApplication instance.</param>
        public static async Task InitialDatabaseAsync(this WebApplication app)
        {
            // Create a new scope to retrieve services
            using var scope = app.Services.CreateScope();
            // Get the ApplicationDbContext from the service provider
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            // Apply any pending migrations before seeding data
            if (context.Database.GetPendingMigrations().Any())
            {
                // Synchronously applies migrations
                context.Database.MigrateAsync().GetAwaiter().GetResult();
            }
            // Seed the database with initial data
            await SeedAsync(context);
        }
        /// <summary>
        /// Calls individual seed methods to populate the database with initial data.
        /// </summary>
        /// <param name="context">The database context.</param>
        private static async Task SeedAsync(ApplicationDbContext context)
        {
            await SeedCustomerAsync(context);
            await SeedProductAsync(context);
            await SeedOrderAndItemAsync(context);
        }
        /// <summary>
        /// Seeds the Customers table with initial data if it's empty.
        /// </summary>
        /// <param name="context">The database context.</param>
        private static async Task SeedCustomerAsync(ApplicationDbContext context)
        { 
            // Check if the Customers table is empty before inserting data
            if (!await context.Customers.AnyAsync())
            { 
                // Add initial customer data
                await context.Customers.AddRangeAsync(InitialData.Customers);
                // Save changes to the database
                await context.SaveChangesAsync();
            } 
        }
        /// <summary>
        /// Seeds the Products table with initial data if it's empty.
        /// </summary>
        /// <param name="context">The database context.</param>
        private static async Task SeedProductAsync(ApplicationDbContext context)
        {
            if (!await context.Products.AnyAsync())
            {
                await context.Products.AddRangeAsync(InitialData.Products);
                await context.SaveChangesAsync();
            }
        }
        /// <summary>
        /// Seeds the Orders and OrderItems tables with initial data if they're empty.
        /// </summary>
        /// <param name="context">The database context.</param>
        private static async Task SeedOrderAndItemAsync(ApplicationDbContext context)
        {
            if (!await context.Orders.AnyAsync())
            {
                await context.Orders.AddRangeAsync(InitialData.OrdersWithItem);
                await context.SaveChangesAsync();
            }
        }
    }
}
