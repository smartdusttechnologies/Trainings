namespace Catalog.API.Models.DTO.GetDto
{
     public record GetProductRequest(int? PageNumber = 1, int? PageSize = 10);
}
