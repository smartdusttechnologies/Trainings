using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Catalog.API.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageFile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "ImageFile", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("1a5d52a2-7c21-43d0-a764-cf3f2325703d"), "[\"Tablet\",\"Electronics\"]", "Samsung's premium tablet, perfect for work and play.", "product-10.png", "Samsung Galaxy Tab S7", 800.00m },
                    { new Guid("2a3f206f-0fd7-44b6-939b-9351c9c9843c"), "[\"Accessories\",\"Electronics\"]", "A high-performance wireless mouse designed for professionals.", "product-13.png", "Logitech MX Master 3 Mouse", 99.99m },
                    { new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8"), "[\"White Appliances\"]", "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.", "product-3.png", "Huawei Plus", 650.00m },
                    { new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"), "[\"Smart Phone\"]", "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.", "product-1.png", "IPhone X", 950.00m },
                    { new Guid("53fcd7d7-5e4a-47e5-bd84-eec179ea2429"), "[\"Accessories\",\"Audio\"]", "Industry-leading noise-canceling headphones from Sony.", "product-15.png", "Sony WH-1000XM4", 350.00m },
                    { new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27"), "[\"White Appliances\"]", "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.", "product-4.png", "Xiaomi Mi 9", 470.00m },
                    { new Guid("728b6297-7d24-466d-bb74-8c4099f67655"), "[\"Accessories\",\"Audio\"]", "Wireless noise-canceling headphones with world-class sound quality.", "product-14.png", "Bose QuietComfort 35 II", 299.00m },
                    { new Guid("72a9b04e-9d2a-47c3-9f6f-b396e17b1fe3"), "[\"Laptop\",\"Electronics\"]", "The latest MacBook Pro, featuring Apple's M1 chip for incredible performance.", "product-7.png", "MacBook Pro 16\"", 2200.00m },
                    { new Guid("93170c85-7795-489c-8e8f-7dcf3b4f4188"), "[\"Camera\"]", "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.", "product-6.png", "Panasonic Lumix", 240.00m },
                    { new Guid("b786103d-c621-4f5a-b498-23452610f88c"), "[\"Smart Phone\"]", "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.", "product-5.png", "HTC U11+ Plus", 380.00m },
                    { new Guid("c06ec38f-70b1-4727-b94d-ff10db8a1bb3"), "[\"Wearable\",\"Health \\u0026 Fitness\"]", "Track your fitness and health with the Fitbit Charge 5.", "product-12.png", "Fitbit Charge 5", 150.00m },
                    { new Guid("c4bbc4a2-4555-45d8-97cc-2a99b2167bff"), "[\"Home Kitchen\"]", "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.", "product-6.png", "LG G7 ThinQ", 240.00m },
                    { new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"), "[\"Smart Phone\"]", "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.", "product-2.png", "Samsung 10", 840.00m },
                    { new Guid("d2e907b2-b321-4195-8c7a-b9f7e2c88d56"), "[\"Wearable\",\"Electronics\"]", "The latest smartwatch from Apple, offering enhanced health tracking and faster charging.", "product-11.png", "Apple Watch Series 7", 399.00m },
                    { new Guid("dbc8d8e9-9a69-4204-810b-2bc9cd351702"), "[\"Tablet\",\"Electronics\"]", "The 12.9-inch iPad Pro features a Liquid Retina display and is perfect for creative professionals.", "product-9.png", "Apple iPad Pro 12.9\"", 1100.00m },
                    { new Guid("e2f0c575-49f7-47fe-b935-59c0e9e9a4fd"), "[\"Laptop\",\"Electronics\"]", "A slim and powerful ultrabook with a high-definition screen.", "product-8.png", "Dell XPS 13", 1500.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
