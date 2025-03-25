using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.ValueObject
{
    public record OrderName
    {
        private const int DefaultLength = 5;
        public string Value { get; }
        private OrderName(string value) => Value = value;

        public static OrderName Of(string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            //if (value.Length <= DefaultLength)
            //{
            //    throw new ArgumentOutOfRangeException(nameof(value), $"Value must be greater than {DefaultLength} characters.");
            //}

            return new OrderName(value);
        }
    }
}
