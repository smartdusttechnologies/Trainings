namespace Ordering.Domain.ValueObject
{
    /// <summary>
    /// Represents an immutable Address value object in the domain.
    /// This is used to store customer address-related details.
    /// </summary>
    public record Address
    {
        /// <summary>
        /// Gets the first name of the address owner.
        /// </summary>
        public string FirstName { get; } = default;
        /// <summary>
        /// Gets the last name of the address owner.
        /// </summary>
        public string LastName { get; } = default;
        /// <summary>
        /// Gets the email address of the recipient (optional).
        /// </summary>
        public string? EmailAdress { get; } = default;
        /// <summary>
        /// Gets the main address line (street, house number, etc.).
        /// </summary>
        public string AdressLine { get; } = default;
        /// <summary>
        /// Gets the country name of the address.
        /// </summary>
        public string Country { get; } = default;
        /// <summary>
        /// Gets the state or province of the address.
        /// </summary>
        public string State { get; } = default;
        /// <summary>
        /// Gets the ZIP or postal code of the address.
        /// </summary>
        public string ZipCode { get; } = default;
        /// <summary>
        /// Protected parameterless constructor to prevent direct instantiation.
        /// This is required by ORM tools or serialization.
        /// </summary>
        protected Address()
        {

        }
        /// <summary>
        /// Private constructor to initialize an Address object with provided values.
        /// </summary>
        /// <param name="firstName">First name of the recipient.</param>
        /// <param name="lastName">Last name of the recipient.</param>
        /// <param name="emailAdress">Email address of the recipient (optional).</param>
        /// <param name="adressLine">Street address or main address line.</param>
        /// <param name="country">Country of the address.</param>
        /// <param name="state">State or province of the address.</param>
        /// <param name="zipCode">ZIP or postal code.</param>
        private Address(string firstName, string lastName, string? emailAdress, string adressLine, string country, string state, string zipCode)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAdress = emailAdress;
            AdressLine = adressLine;
            Country = country;
            State = state;
            ZipCode = zipCode;
        }
        /// <summary>
        /// Factory method to create an instance of <see cref="Address"/>.
        /// Ensures that required fields are not null or empty.
        /// </summary>
        /// <param name="firstName">First name of the recipient.</param>
        /// <param name="lastName">Last name of the recipient.</param>
        /// <param name="emailAdress">Email address of the recipient.</param>
        /// <param name="adressLine">Street address or main address line.</param>
        /// <param name="country">Country of the address.</param>
        /// <param name="state">State or province of the address.</param>
        /// <param name="zipCode">ZIP or postal code.</param>
        /// <returns>A new <see cref="Address"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when email or address line is empty or null.</exception>

        public static Address Of(string firstName, string lastName, string? emailAdress, string adressLine, string country, string state, string zipCode)
        {  
            // Ensure email address and address line are not empty
            ArgumentException.ThrowIfNullOrWhiteSpace(emailAdress);
            ArgumentException.ThrowIfNullOrWhiteSpace(adressLine);

            return new Address(firstName, lastName, emailAdress, adressLine, country, state, zipCode);
        }
    }
}
