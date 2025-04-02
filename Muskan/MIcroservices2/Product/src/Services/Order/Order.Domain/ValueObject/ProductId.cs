using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.ValueObject
{
    /// <summary>
    /// Value Object for ProductID
    /// </summary>
    public record ProductId
    {
        public Guid Value { get; }
        private ProductId(Guid value) => Value = value;


        public static ProductId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("OrderItem id cannot be empty");
            }

            return new ProductId(value);

        }
    }
}
