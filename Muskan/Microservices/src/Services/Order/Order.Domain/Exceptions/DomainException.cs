namespace Ordering.Domain.Exceptions
{
    /// <summary>
    /// Represents a custom exception specific to the domain layer of the application.
    /// This exception is thrown when a domain-related business rule is violated.
    /// </summary>
    public class DomainException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class.
        /// </summary>
        /// <param name="message">The error message that describes the exception.</param>
        public DomainException(string message) :  base($"Domain Exception \"{message}\" thows from Domain layer")
      
        {
            
        }
    }
}
