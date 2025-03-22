namespace Ordering.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configuration class for the OrderItem entity.
    /// Defines the table structure, relationships, and constraints.
    /// </summary>
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        /// <summary>
        /// Configures the properties and relationships of the OrderItem entity.
        /// </summary>
        /// <param name="builder">Entity type builder for configuring the OrderItem entity.</param>

        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // Sets the primary key for the OrderItem entity
            builder.HasKey(x => x.Id);
            // Configures the Id property to be stored as a simple value in the database
            builder.Property(x => x.Id).HasConversion(
                // Converts OrderItemId object to a database-friendly value
                orderItemId => orderItemId.Value,
                // Converts database value back to OrderItemId object
                dbId => OrderItemId.Of(dbId)
                );
            // Defines a relationship: One Product can be associated with many OrderItems
            builder.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId);

            builder.Property(oi => oi.Quantity).IsRequired();
            builder.Property(oi => oi.Price).IsRequired();
        }
    }
}
