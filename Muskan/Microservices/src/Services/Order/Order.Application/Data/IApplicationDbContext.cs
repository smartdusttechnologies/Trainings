namespace Ordering.Application.Data
{
    /// <summary>
    /// IApplicationDbContext interface 
    /// </summary>
    public interface IApplicationDbContext
    {
        /// <summary>
        /// Gets the Customers DbSet.
        /// </summary>
        DbSet<Customer> Customers { get; }

        /// <summary>
        /// Gets the Products DbSet.
        /// </summary>
        DbSet<Product> Products { get; }

        /// <summary>
        /// Gets the Orders DbSet.
        /// </summary>
        DbSet<Orders> Orders { get; }

        /// <summary>
        /// Gets the OrderItems DbSet.
        /// </summary>
        DbSet<OrderItem> OrderItems { get; }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
