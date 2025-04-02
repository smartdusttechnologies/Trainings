namespace Catalog.API.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void SeedProducts(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"),
                    Name = "IPhone X",
                    Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    ImageFile = "product-1.png",
                    Price = 950.00M,
                    Category = new List<string> { "Smart Phone" }
                },
                new Product
                {
                    Id = new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"),
                    Name = "Samsung 10",
                    Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    ImageFile = "product-2.png",
                    Price = 840.00M,
                    Category = new List<string> { "Smart Phone" }
                },
                new Product
                {
                    Id = new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8"),
                    Name = "Huawei Plus",
                    Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    ImageFile = "product-3.png",
                    Price = 650.00M,
                    Category = new List<string> { "White Appliances" }
                },
                new Product
                {
                    Id = new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27"),
                    Name = "Xiaomi Mi 9",
                    Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    ImageFile = "product-4.png",
                    Price = 470.00M,
                    Category = new List<string> { "White Appliances" }
                },
                new Product
                {
                    Id = new Guid("b786103d-c621-4f5a-b498-23452610f88c"),
                    Name = "HTC U11+ Plus",
                    Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    ImageFile = "product-5.png",
                    Price = 380.00M,
                    Category = new List<string> { "Smart Phone" }
                },
                new Product
                {
                    Id = new Guid("c4bbc4a2-4555-45d8-97cc-2a99b2167bff"),
                    Name = "LG G7 ThinQ",
                    Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    ImageFile = "product-6.png",
                    Price = 240.00M,
                    Category = new List<string> { "Home Kitchen" }
                },
                new Product
                {
                    Id = new Guid("93170c85-7795-489c-8e8f-7dcf3b4f4188"),
                    Name = "Panasonic Lumix",
                    Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    ImageFile = "product-6.png",
                    Price = 240.00M,
                    Category = new List<string> { "Camera" }
                },
                 new Product
                 {
                     Id = new Guid("72a9b04e-9d2a-47c3-9f6f-b396e17b1fe3"),
                     Name = "MacBook Pro 16\"",
                     Description = "The latest MacBook Pro, featuring Apple's M1 chip for incredible performance.",
                     ImageFile = "product-7.png",
                     Price = 2200.00M,
                     Category = new List<string> { "Laptop", "Electronics" }
                 },
                new Product
                {
                    Id = new Guid("e2f0c575-49f7-47fe-b935-59c0e9e9a4fd"),
                    Name = "Dell XPS 13",
                    Description = "A slim and powerful ultrabook with a high-definition screen.",
                    ImageFile = "product-8.png",
                    Price = 1500.00M,
                    Category = new List<string> { "Laptop", "Electronics" }
                },
                new Product
                {
                    Id = new Guid("dbc8d8e9-9a69-4204-810b-2bc9cd351702"),
                    Name = "Apple iPad Pro 12.9\"",
                    Description = "The 12.9-inch iPad Pro features a Liquid Retina display and is perfect for creative professionals.",
                    ImageFile = "product-9.png",
                    Price = 1100.00M,
                    Category = new List<string> { "Tablet", "Electronics" }
                },
                new Product
                {
                    Id = new Guid("1a5d52a2-7c21-43d0-a764-cf3f2325703d"),
                    Name = "Samsung Galaxy Tab S7",
                    Description = "Samsung's premium tablet, perfect for work and play.",
                    ImageFile = "product-10.png",
                    Price = 800.00M,
                    Category = new List<string> { "Tablet", "Electronics" }
                },
                new Product
                {
                    Id = new Guid("d2e907b2-b321-4195-8c7a-b9f7e2c88d56"),
                    Name = "Apple Watch Series 7",
                    Description = "The latest smartwatch from Apple, offering enhanced health tracking and faster charging.",
                    ImageFile = "product-11.png",
                    Price = 399.00M,
                    Category = new List<string> { "Wearable", "Electronics" }
                },
                new Product
                {
                    Id = new Guid("c06ec38f-70b1-4727-b94d-ff10db8a1bb3"),
                    Name = "Fitbit Charge 5",
                    Description = "Track your fitness and health with the Fitbit Charge 5.",
                    ImageFile = "product-12.png",
                    Price = 150.00M,
                    Category = new List<string> { "Wearable", "Health & Fitness" }
                },
                new Product
                {
                    Id = new Guid("2a3f206f-0fd7-44b6-939b-9351c9c9843c"),
                    Name = "Logitech MX Master 3 Mouse",
                    Description = "A high-performance wireless mouse designed for professionals.",
                    ImageFile = "product-13.png",
                    Price = 99.99M,
                    Category = new List<string> { "Accessories", "Electronics" }
                },
                new Product
                {
                    Id = new Guid("728b6297-7d24-466d-bb74-8c4099f67655"),
                    Name = "Bose QuietComfort 35 II",
                    Description = "Wireless noise-canceling headphones with world-class sound quality.",
                    ImageFile = "product-14.png",
                    Price = 299.00M,
                    Category = new List<string> { "Accessories", "Audio" }
                },
                new Product
                {
                    Id = new Guid("53fcd7d7-5e4a-47e5-bd84-eec179ea2429"),
                    Name = "Sony WH-1000XM4",
                    Description = "Industry-leading noise-canceling headphones from Sony.",
                    ImageFile = "product-15.png",
                    Price = 350.00M,
                    Category = new List<string> { "Accessories", "Audio" }
                });
        }
    }
}