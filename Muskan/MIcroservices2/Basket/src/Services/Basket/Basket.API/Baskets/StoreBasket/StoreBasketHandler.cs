using Discount.Grpc;

namespace Basket.API.Baskets.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string userName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null.");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("Username cannot be null or empty.")  .When(x => x.Cart is not null);
        }
    }
    public class StoreBasketCommandHandler(IBasketRepository repository , DiscountProtoService.DiscountProtoServiceClient discountPrice) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {

        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            if (command.Cart.Items == null || !command.Cart.Items.Any())
            {               
                Console.WriteLine("Items list is empty or null.");
            }
            await DeductDiscount(command.Cart, cancellationToken);
            await repository.StoreBasket(command.Cart);
            return new StoreBasketResult(command.Cart.UserName);
        }
        private async Task DeductDiscount(ShoppingCart cart , CancellationToken cancellationToken)
        {
            foreach (var item in cart.Items)
            {
                var coupon = await discountPrice.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
                item.Price -= coupon.Amount;
            }
        }
    }
}
