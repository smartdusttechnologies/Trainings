namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Orders>
    {
        public void Configure(EntityTypeBuilder<Orders> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(x => x.Id).HasConversion(
                orderItemId => orderItemId.Value,
                dbId => OrderId.Of(dbId));
            //Customer relation One Customer has many orders
            builder.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId);

            //OrderItems relation One Order has many OrderItems
            builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId);

            //OrderName ValueObject
            builder.ComplexProperty(o => o.OrderName, nameBuilder =>
            {
                nameBuilder.Property(n => n.Value)
                    .HasMaxLength(100)                   
                    .IsRequired();
            });


            builder.ComplexProperty(
               o => o.ShippingAdress, addressBuilder =>
               {
                   addressBuilder.Property(a => a.FirstName)
                       .HasMaxLength(50)
                       .IsRequired();

                   addressBuilder.Property(a => a.LastName)
                        .HasMaxLength(50)
                        .IsRequired();

                   addressBuilder.Property(a => a.EmailAdress)
                       .HasMaxLength(50);

                   addressBuilder.Property(a => a.AdressLine)
                       .HasMaxLength(180)
                       .IsRequired();

                   addressBuilder.Property(a => a.Country)
                       .HasMaxLength(50);

                   addressBuilder.Property(a => a.State)
                       .HasMaxLength(50);

                   addressBuilder.Property(a => a.ZipCode)
                       .HasMaxLength(10)
                       .IsRequired();
               });

            builder.ComplexProperty(
              o => o.Billingadress, addressBuilder =>
              {
                  addressBuilder.Property(a => a.FirstName)
                       .HasMaxLength(50)
                       .IsRequired();

                  addressBuilder.Property(a => a.LastName)
                       .HasMaxLength(50)
                       .IsRequired();

                  addressBuilder.Property(a => a.EmailAdress)
                      .HasMaxLength(50);

                  addressBuilder.Property(a => a.AdressLine)
                      .HasMaxLength(180)
                      .IsRequired();

                  addressBuilder.Property(a => a.Country)
                      .HasMaxLength(50);

                  addressBuilder.Property(a => a.State)
                      .HasMaxLength(50);

                  addressBuilder.Property(a => a.ZipCode)
                      .HasMaxLength(10)
                      .IsRequired();
              });

            builder.ComplexProperty(
                   o => o.Payment, paymentBuilder =>
                   {
                       paymentBuilder.Property(p => p.CardName)
                           .HasMaxLength(50);

                       paymentBuilder.Property(p => p.CardNumber)
                           .HasMaxLength(24)
                           .IsRequired();

                       paymentBuilder.Property(p => p.ExpiryDate)
                           .HasMaxLength(10);

                       paymentBuilder.Property(p => p.CVV)
                           .HasMaxLength(3);

                       paymentBuilder.Property(p => p.PaymentMethod);
                   });

            builder.Property(o => o.Status)      
            .HasConversion(
                s => s.ToString(),
                dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus));

            builder.Property(o => o.TotalPrice);

        }
    }
}
