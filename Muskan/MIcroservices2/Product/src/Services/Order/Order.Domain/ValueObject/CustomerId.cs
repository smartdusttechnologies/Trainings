namespace Ordering.Domain.ValueObject
{
    /// <summary>
    /// Customer id value object
    /// </summary>
    public record CustomerId
    {
        /// <summary>
        /// Customer id value
        /// </summary>
        public Guid Value { get;  }
        private CustomerId(Guid value) =>  Value = value;
     

        public static  CustomerId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("Customer id cannot be empty");
            }

            return new CustomerId(value);

        }
    }
}
