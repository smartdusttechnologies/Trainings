namespace Ordering.Domain.Models
{
    /// <summary>
    /// Represents a customer Entity.
    /// That inherits the Entity base class.
    /// For using Entity it provide the common info like ID ,Createdby ,UpdateBy ,CreatedAt ,UpdatedAt
    /// 
    /// </summary>
    public class Customer : Entity<CustomerId>
    {
        /// <summary>
        /// Customer Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Customer Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Create a new customer
        /// Factory method to create a new customer instance.
        /// Ensures name and email are not null or empty.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static Customer Create(CustomerId id,string name ,string email)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            var customer=  new Customer
            {
                Id = id,
                Name = name,
                Email = email
            };
            return customer;
        }

    }
}
