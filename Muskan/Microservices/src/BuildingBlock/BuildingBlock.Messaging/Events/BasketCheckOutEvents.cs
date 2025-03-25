
namespace BuildingBlock.Messaging.Events;

public record  BasketCheckOutEvents : IntegrationEvents
{
    public string Username { get; set; } = default;
    public Guid CustomerId { get; set; } = default;
    public decimal TotalPrice   { get; set; } = default;
    ///Shipping andf Billing Address 
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
    /// </summary>B

    public string? CardName { get; } = default;
    public string CardNumber { get; } = default;
    public string ExpiryDate { get; } = default;
    public string CVV { get; } = default;
    public int PaymentMethod { get; } = default;

}
