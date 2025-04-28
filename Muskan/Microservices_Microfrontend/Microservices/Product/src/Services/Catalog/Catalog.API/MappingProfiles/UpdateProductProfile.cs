
using Catalog.API.Models.DTO.UpdateDto;
using Catalog.API.Products.UpdateProduct;

namespace Catalog.API.MappingProfiles
{
     public class UpdateProductProfile : Profile
     {
          public UpdateProductProfile()
          {
               CreateMap<UpdateProductRequest, UpdateProductCommand>();
               CreateMap<UpdateProductResult, UpdateProductResponse>();
          }
     }
}
