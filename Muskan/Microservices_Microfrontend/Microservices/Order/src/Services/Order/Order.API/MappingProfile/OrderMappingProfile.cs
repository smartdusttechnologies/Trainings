
using Ordering.Application.DTOs.Create;
using Ordering.Application.DTOs.Update;
using Ordering.Application.Order.Commands.CreateOrder;
using Ordering.Application.Order.Commands.UpdateOrder;
using Ordering.Domain.Models;

namespace Ordering.API.MappingProfile
{
     public class OrderMappingProfile : Profile
     {
          public OrderMappingProfile()
          {
               CreateMap<CreateOrderRequest, CreateOrderCommand>()
                   .ForPath(dest => dest.Order.Payment.CVV, opt => opt.MapFrom(src => src.Order.Payment.CVV));

               CreateMap<Guid, CreateOrderResponse>()
                   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));

               CreateMap<Orders, OrdersDTO>()
                          .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                          .ForMember(dest => dest.OrderName, opt => opt.MapFrom(src => src.OrderName.Value));
               CreateMap<UpdateOrderRequest, UpdateOrderCommand>()
                  .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order));

               CreateMap<UpdateOrderResult, UpdateOrderResponse>()
                   .ForMember(dest => dest.IsSuccess, opt => opt.MapFrom(src => src.isSuccess));
          }
     }

}
