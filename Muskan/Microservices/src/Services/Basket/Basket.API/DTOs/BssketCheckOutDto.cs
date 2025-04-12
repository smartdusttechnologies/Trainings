namespace Basket.API.DTOs;

public class BssketCheckOutDto
{
    /// <summary>
    /// Username 
    /// </summary>
    public string Username { get; set; } = default;
    public Guid CustomerId { get; set; } = default;
    public decimal TotalPrice { get; set; } = default;
    //Shipping andf Billing Address     
    /// <summary>
    /// Gets the first name of the address owner.
    /// </summary>
    public string FirstName { get; set; } = default;
    /// <summary>
    /// Gets the last name of the address owner.
    /// </summary>
    public string LastName { get; set; } = default;
    /// <summary>
    /// Gets the email address of the recipient (optional).
    /// </summary>
    public string? EmailAdress { get; set; } = default;
    /// <summary>
    /// Gets the main address line (street, house number, etc.).
    /// </summary>
    public string AdressLine { get; set; } = default;
    /// <summary>
    /// Gets the country name of the address.
    /// </summary>
    public string Country { get; set; } = default;
    /// <summary>
    /// Gets the state or province of the address.
    /// </summary>
    public string State { get; set; } = default;
    /// <summary>
    /// Gets the ZIP or postal code of the address.
    /// </summary>
    public string ZipCode { get; set; } = default;
    /// <summary>
    /// Protected parameterless constructor to prevent direct instantiation.
    /// This is required by ORM tools or serialization.
    /// </summary>
    
    // Payment
    public string? CardName { get; set; } = default;
    public string CardNumber { get; set; } = default;
    public string ExpiryDate { get; set; } = default;
    public string CVV { get; set; } = default;
    public int PaymentMethod { get; set; } = default;

}
