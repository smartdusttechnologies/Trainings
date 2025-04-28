using Catalog.API.Models.DTO.DeleteDto;
using Catalog.API.Models.DTO.GetBYCategory;
using Catalog.API.Models.DTO.GetByIdDto;
using Catalog.API.Products.DeleteProduct;
using Catalog.API.Products.GetProductByCategory;
using Catalog.API.Products.GetProductById;

namespace Catalog.API.MappingProfiles
{
     public class GetProductByIdProfile : Profile
     {
          public GetProductByIdProfile()
          {
               CreateMap<GetProductByIdResult, GetProductByIdResponse>()
                   .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

               //Get Product By Category
               CreateMap<GetProductByCategoryResult, GetProductByCategoryResponse>();
               CreateMap<DeleteProductResult, DeleteProductResponse>();
          }
     }
}
