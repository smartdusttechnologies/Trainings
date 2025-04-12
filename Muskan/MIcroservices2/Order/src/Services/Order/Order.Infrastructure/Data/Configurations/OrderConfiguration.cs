using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Ordering.Domain.Abstraction;
using System.Diagnostics.Metrics;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ordering.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configuration class for the Orders entity.
    /// Defines the table structure, relationships, and constraints.
    /// </summary>
    public class OrderConfiguration : IEntityTypeConfiguration<Orders>
    {  
        /// <summary>
       /// Configures the properties and relationships of the Orders entity.
       /// </summary>
       /// <param name="builder">Entity type builder for configuring the Orders entity.</param>

        public void Configure(EntityTypeBuilder<Orders> builder)
        { 
            // Sets the primary key for the Orders entity
            builder.HasKey(o => o.Id);
            // Configures the Id property to be stored as a simple value in the database
            builder.Property(x => x.Id).HasConversion(
                // Converts OrderId object to a database-friendly value
                orderItemId => orderItemId.Value,
                // Converts database value back to OrderId object
                dbId => OrderId.Of(dbId));
            //Customer relation One Customer has many orders
            // Defines a relationship: One Customer can have many Orders
            builder.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId);

            //OrderItems relation One Order has many OrderItems
            // Defines a relationship: One Order can have many OrderItems
            builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId);

            //OrderName ValueObject
            // Configures OrderName as a complex value object with a max length of 100 characters

            builder.ComplexProperty(o => o.OrderName, nameBuilder =>
            {
                nameBuilder.Property(n => n.Value)
                // Ensures OrderName does not exceed 100 characters
                    .HasMaxLength(100)
                    // Ensures OrderName cannot be null
                    .IsRequired();
            });

            // Configures Shipping Address properties as a complex value object
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
            // Configures Billing Address properties as a complex value object
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
            // Configures Payment details as a complex value object
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
            // Converts OrderStatus enum to a string for database storage
            builder.Property(o => o.Status)      
            .HasConversion(
                // Converts OrderStatus enum to string before saving
                s => s.ToString(),
                // Converts string back to OrderStatus enum
                dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus));
            // Configures the TotalPrice property
            builder.Property(o => o.TotalPrice);

        }
    }
}
/// <summary>
/// ComplexProperty in Entity Framework Core
/// ComplexProperty is a new feature introduced in EF Core 8 that allows you to map complex types (value objects) directly in the database without requiring a separate entity table.
///</summary>


/// <summary>
/// Why Use ComplexProperty?
/// Encapsulating Value Objects
/// In Domain-Driven Design (DDD), you may have Value Objects (like Address or PaymentDetails) that do not have an identity and should not be stored as separate entities.
/// ComplexProperty helps model such nested properties inside an entity without creating a separate table.
/// Avoids Extra Tables
/// In older EF versions, you had to create a separate entity and a one-to-one relationship to store complex data.
/// With ComplexProperty, all the fields are stored in the same table as the parent entity.
/// Easier to Manage & Query
/// Since complex properties are embedded within the same table, there is no need for joins, making queries faster and simpler.
/// </summary>


/// <summary>
/// How to Use ComplexProperty?
/// 1. Let's assume we have an Order entity with an Address as a value object.
/// No Id property (because it’s a value object, not an entity).

/// 2. Use ComplexProperty in the Entity Configuration

///        // Configuring Address as a complex property
///        builder.ComplexProperty(o => o.ShippingAddress, addressBuilder =>
///       {
///            addressBuilder.Property(a => a.FirstName).HasMaxLength(50).IsRequired();
///           addressBuilder.Property(a => a.LastName).HasMaxLength(50).IsRequired();
///            other properties ...
///       });


/// 3.Generated Table(Order Table)
/// After migration, the Order table will contain all Address fields inside the same table, instead of creating a separate Address table.
/// </summary>

/// <summary>
/// When to Use ComplexProperty?
/// Use ComplexProperty When...	Do Not Use ComplexProperty When...
/// You have a value object (like Address, PaymentDetails).	The object has an identity (ID) and should be an entity.
/// You want to store fields inside the same table.	You need separate table relationships (e.g., One-to-Many).
/// You don’t need direct references to the object in other entities.	You need to query the object separately or use foreign keys.
/// </summary>

/// <summary>
/// Alternatives to ComplexProperty
/// If ComplexProperty is not suitable for your case, you can use:
/// 1.Separate Entity with Foreign Key
/// Instead of embedding the address in the Order table, create a separate table and use a foreign key.


/// public class Address
/// {
///    public int Id { get; set; }  // Address becomes an entity
///    public string FirstName { get; set; }
///    other fields...
/// }

/// public class Order
/// {
///    public int Id { get; set; }
///    public int AddressId { get; set; }  // Foreign key
///    public Address Address { get; set; }
/// }


/// 2. JSON Column (For NoSQL-like Storage)
/// If using SQL Server 2016 + or PostgreSQL, you can store the Address as JSON instead of using ComplexProperty.

/// builder.Property(o => o.Address)
///    .HasConversion(
///        address => JsonSerializer.Serialize(address, new JsonSerializerOptions()),
///        json => JsonSerializer.Deserialize<Address>(json, new JsonSerializerOptions())
///    )
///    .HasColumnType("jsonb"); // Use "nvarchar(max)" for SQL Server
/// Pros: More flexible, allows storing dynamic properties.
/// Cons: Harder to query using SQL.
/// </summary>
/// </summary>

