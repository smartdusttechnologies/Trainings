using Catalog.API.Models.DTO.CreateDto;
using Catalog.API.Products.CreateProduct;

namespace Catalog.API.MappingProfiles
{

     public class ProductProfile : Profile
     {
          public ProductProfile()
          {
               CreateMap<CreateProductRequest, CreateProductCommand>();
               CreateMap<CreateProductResult, CreateProductResponse>();
          }
     }
}
