namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
          builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion(
                orderItemId => orderItemId.Value,
                dbId => OrderItemId.Of(dbId)
                );
            builder.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId);

            builder.Property(oi => oi.Quantity).IsRequired();
            builder.Property(oi => oi.Price).IsRequired();
        }
    }
}
