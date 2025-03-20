using System;
using System.Collections.Generic;
namespace Ordering.Domain.ValueObject
{
    public record Address
    {
        public string FirstName { get; } = default;
        public string LastName { get; } = default;
        public string? EmailAdress { get; } = default;
        public string AdressLine { get; } = default;
        public string Country { get; } = default;
        public string State { get; } = default;
        public string ZipCode { get; } = default;

        protected Address()
        {

        }
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
        public static Address Of(string firstName, string lastName, string? emailAdress, string adressLine, string country, string state, string zipCode)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(emailAdress);
            ArgumentException.ThrowIfNullOrWhiteSpace(adressLine);

            return new Address(firstName, lastName, emailAdress, adressLine, country, state, zipCode);
        }
    }
}
