namespace Basket.API.Baskets.StoreBasket
{
     public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
     public record StoreBasketResult(string userName);

     public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
     {
          public StoreBasketCommandValidator()
          {
               RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null.");
               RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("Username cannot be null or empty.").When(x => x.Cart is not null);
          }
     }
     public class StoreBasketCommandHandler(IBasketRepository repository, ILoggingService<StoreBasketCommand> logger, DiscountProtoService.DiscountProtoServiceClient discountPrice) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
     {

          public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
          {
               if (command.Cart.Items == null || !command.Cart.Items.Any())
               {
                    await logger.LogWarningAsync("Cart is empty or null.");

               }

               await logger.LogInformationAsync("Storing basket for user: " + command.Cart.UserName);
               await DeductDiscount(command.Cart, cancellationToken);
               await repository.StoreBasket(command.Cart);
               return new StoreBasketResult(command.Cart.UserName);




          }
          private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
          {
               await logger.LogInformationAsync("Deducting discount for user: " + cart.UserName);

               foreach (var item in cart.Items)
               {
                    await logger.LogInformationAsync("Deducting discount for item: " + item.ProductName);
                    var coupon = await discountPrice.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
                    item.Price -= coupon.Amount;
               }
          }
     }
}
