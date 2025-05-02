using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Basket.API.Models
{
     public class ShoppingCartItem
     {
          public Guid Id { get; set; }
          public int Quantity { get; set; }
          public string Color { get; set; }
          public decimal Price { get; set; }
          public Guid ProductId { get; set; }
          public string ProductName { get; set; }
          public Guid ShoppingCartId { get; set; }
          [JsonIgnore]
          [ValidateNever]
          public ShoppingCart ShoppingCart { get; set; }
     }
}
