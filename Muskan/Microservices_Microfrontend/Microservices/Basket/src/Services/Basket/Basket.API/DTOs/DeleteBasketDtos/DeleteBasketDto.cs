using Basket.API.Baskets.DeleteBasket;

namespace Basket.API.DTOs.DeleteBasketDtos
{
     public record DeleteBasketRequest(string UserName) : ICommand<DeleteBasketResult>;
     public record DeleteBasketResponse(bool isSuccess);
}
