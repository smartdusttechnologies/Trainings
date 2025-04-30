using Basket.API.Baskets.StoreBasket;

namespace Basket.API.DTOs.StoreBasketDtos
{
     public record StoreBasketRequest(ShoppingCart Cart) : ICommand<StoreBasketResult>;
     public record StoreBasketResponse(string UserName);

}
