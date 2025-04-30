namespace Basket.API.Baskets.GetBasket
{
     public record GetBasketQuery(string userName) : IQuery<GetBasketResult>;
     public record GetBasketResult(ShoppingCart Cart);
     public class GetBasketQueryHandler(IBasketRepository repository, ILoggingService<GetBasketQueryHandler> logger) : IQueryHandler<GetBasketQuery, GetBasketResult>
     {
          public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
          {
               await logger.LogInformationAsync($"Get Basket {query}");

               var basket = await repository.GetBasket(query.userName);

               return new GetBasketResult(basket);
          }
     }
}
