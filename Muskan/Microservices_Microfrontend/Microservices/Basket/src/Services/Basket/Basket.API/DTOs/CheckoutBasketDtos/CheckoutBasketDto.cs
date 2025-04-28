namespace Basket.API.DTOs.CheckoutBasketDtos
{
     public record CheckOutBasketRequest(BssketCheckOutDto BasketCheckOutDto);
     public record CheckOutBasketResponse(bool isSuccess);
}
