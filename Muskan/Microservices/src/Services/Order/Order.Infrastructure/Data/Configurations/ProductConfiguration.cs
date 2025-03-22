namespace Ordering.Infrastructure.Data.Configurations
{
    /// <summary>
    ///  Configuration class for the Product entity.
    /// </summary>
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        /// <summary>
        /// Configures the properties and relationships of the Product entity.
        /// </summary>
        /// <param name="builder">Entity type builder for configuring the OrderItem entity.</param>
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasConversion(
                customerId => customerId.Value,
                dbId => ProductId.Of(dbId)
                );
            builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
           
        }
    }
}
