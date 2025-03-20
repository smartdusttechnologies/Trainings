namespace Ordering.Domain.ValueObject
{
    public record Payment
    {

        public string? CardName { get; }
        public string CardNumber { get; }
        public string ExpiryDate { get; }
        public string CVV { get; }
        public int PaymentMethod { get; } = default;

        protected Payment()
        {
        }
        private Payment(string cardName, string cardNumber, string expiryDate, string cvv, int paymentMethod)
        {
            CardName = cardName;
            CardNumber = cardNumber;
            ExpiryDate = expiryDate;
            CVV = cvv;
            PaymentMethod = paymentMethod;
        }
        public static Payment Of(string cardName, string cardNumber, string expiryDate, string cvv, int paymentMethod)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(cardName);
            ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);

            ArgumentException.ThrowIfNullOrWhiteSpace(cvv);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3);
            return new Payment(cardName, cardNumber, expiryDate, cvv, paymentMethod);

        }
    }
}