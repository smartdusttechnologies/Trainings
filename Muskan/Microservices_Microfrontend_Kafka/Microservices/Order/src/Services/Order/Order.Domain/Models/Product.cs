namespace Ordering.Domain.Models
{
    /// <summary>
    /// Represents a product Entity.
    /// </summary>
    public class Product : Entity<ProductId>
    {
        /// <summary>
        /// Product Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Product Price
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public static Product Create(ProductId id, string name, decimal price)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
            var product = new Product
            {
                Id = id,
                Name = name,
                Price = price
            };
            return product;
        }
    }
}
