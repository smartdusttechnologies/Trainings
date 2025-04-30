using Basket.API.Baskets.UpdateBasket;

namespace Basket.API.DTOs.UpdateBasketDtos
{
     public record UpdateBasketRequest(ShoppingCart Cart) : ICommand<UpdateBasketResult>;
     public record UpdateBasketResponse(bool isSuccess);
}
