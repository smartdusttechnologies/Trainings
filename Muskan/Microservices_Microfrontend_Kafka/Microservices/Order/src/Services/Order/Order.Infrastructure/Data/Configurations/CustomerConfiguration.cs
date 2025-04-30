namespace Ordering.Infrastructure.Data.Configurations
{   
    /// <summary>
    /// Configuration class for the Customer entity.
    /// This class is used to define the database table structure and constraints.
    /// </summary>
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        /// <summary>
        /// Configures the properties and constraints of the Customer entity.
        /// </summary>
        /// <param name="builder">The entity type builder used to configure the Customer entity.</param>

        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // Sets the primary key for the Customer entity
            builder.HasKey(c => c.Id);
            // Configures the Id property to be stored as a simple value in the database
            builder.Property(c => c.Id).HasConversion(
                // Converts CustomerId object to a database-friendly value
                customerId => customerId.Value,
                // Converts database value back to CustomerId object
                dbId => CustomerId.Of(dbId)
                );
            // Configures the Name property with a maximum length of 100 characters and makes it required
            // Ensures Name does not exceed 100 characters
            // Ensures Name cannot be null
            builder.Property(c => c.Name).HasMaxLength(100).IsRequired();

            // Configures the Email property with a maximum length of 255 characters and makes it required
            // Ensures Email does not exceed 255 characters
            // Ensures Email cannot be null
            builder.Property(c => c.Email).HasMaxLength(255).IsRequired();
            // Adds a unique index on the Email property to prevent duplicate email addresses

            builder.HasIndex(c => c.Email).IsUnique();
        }
    }
}
