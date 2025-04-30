namespace Catalog.API.Models.DTO.CreateDto
{
     public class CreateProductRequest
     {
          public string Name { get; set; }
          public List<string> Category { get; set; }
          public string Description { get; set; }
          public decimal Price { get; set; }
          public string Image { get; set; }
     }
}
