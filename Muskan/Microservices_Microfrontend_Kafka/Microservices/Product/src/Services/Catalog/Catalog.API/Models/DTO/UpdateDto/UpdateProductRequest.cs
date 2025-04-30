namespace Catalog.API.Models.DTO.UpdateDto
{
     public record UpdateProductRequest(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price);

}
