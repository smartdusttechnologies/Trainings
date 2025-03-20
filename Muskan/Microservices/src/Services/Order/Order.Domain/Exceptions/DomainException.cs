namespace Ordering.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message) :  base($"Domain Exception \"{message}\" thows from Domain layer")
      
        {
            
        }
    }
}
