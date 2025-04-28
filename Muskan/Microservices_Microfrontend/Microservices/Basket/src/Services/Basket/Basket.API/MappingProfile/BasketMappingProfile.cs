using Basket.API.Baskets.CheckOutBasket;
using Basket.API.Baskets.DeleteBasket;
using Basket.API.Baskets.GetBasket;
using Basket.API.Baskets.StoreBasket;
using Basket.API.Baskets.UpdateBasket;
using Basket.API.DTOs.CheckoutBasketDtos;
using Basket.API.DTOs.DeleteBasketDtos;
using Basket.API.DTOs.GetBasketDtos;
using Basket.API.DTOs.StoreBasketDtos;
using Basket.API.DTOs.UpdateBasketDtos;
using BuildingBlock.Messaging.Events;
namespace Basket.API.MappingProfile
{

     public class BasketMappingProfile : Profile
     {
          public BasketMappingProfile()
          {
               //Store Basket
               CreateMap<StoreBasketRequest, StoreBasketCommand>()
                   .ForCtorParam("cart", opt => opt.MapFrom(src => src.Cart));

               CreateMap<StoreBasketResult, StoreBasketResponse>();
               //GetBasket Mapping 
               CreateMap<GetBasketResult, GetBasketResponse>();
               // Update Basket 
               CreateMap<UpdateBasketRequest, UpdateBasketCommand>()
               .ForCtorParam("cart", opt => opt.MapFrom(src => src.Cart));

               CreateMap<UpdateBasketResult, UpdateBasketResponse>();
               //Delete Basket 
               CreateMap<DeleteBasketResult, DeleteBasketResponse>();

               //Checkout Basket 
               CreateMap<CheckOutBasketRequest, CheckOutBasketCommand>()
                  .ForMember(dest => dest.BasketCheckOutDto, opt => opt.MapFrom(src => src.BasketCheckOutDto));

               CreateMap<CheckOutBasketResult, CheckOutBasketResponse>()
                   .ForMember(dest => dest.isSuccess, opt => opt.MapFrom(src => src.isSuccess));
               //basket Checkout Event 

               CreateMap<BssketCheckOutDto, BasketCheckOutEvents>()
                   .ForMember(dest => dest.TotalPrice, opt => opt.Ignore());
          }
     }
}
