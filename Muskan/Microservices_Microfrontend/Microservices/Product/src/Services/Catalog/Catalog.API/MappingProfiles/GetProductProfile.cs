using Catalog.API.Models.DTO.GetDto;
using Catalog.API.Products.GetProduct;

namespace Catalog.API.MappingProfiles
{
     public class GetProductProfile : Profile
     {
          public GetProductProfile()
          {
               CreateMap<GetProductResult, GetProductResponse>();

          }
     }
}
