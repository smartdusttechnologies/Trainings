using Discount.Grpc;

namespace Basket.API.Baskets.UpdateBasket
{
    public record UpdateBasketCommand(ShoppingCart Cart) : ICommand<UpdateBasketResult>;
    public record UpdateBasketResult(bool isSuccess);

    public class UpdateBasketCommandValidator : AbstractValidator<UpdateBasketCommand>
    {
        public UpdateBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart cannot be null.");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("Username cannot be null or empty.")
                .When(x => x.Cart is not null);
        }
    }

    public class UpdateBasketCommandHandler(IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountService)
        : ICommandHandler<UpdateBasketCommand, UpdateBasketResult>
    {
        public async Task<UpdateBasketResult> Handle(UpdateBasketCommand command, CancellationToken cancellationToken)
        {
            if (command.Cart.Items == null || !command.Cart.Items.Any())
            {
                Console.WriteLine("Items list is empty or null.");
            }

            await ApplyDiscounts(command.Cart, cancellationToken);
            await repository.UpdateBasket(command.Cart);
            return new UpdateBasketResult(true);
        }

        private async Task ApplyDiscounts(ShoppingCart cart, CancellationToken cancellationToken)
        {
            foreach (var item in cart.Items)
            {
                var discount = await discountService.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
                item.Price -= discount.Amount;
            }
        }
    }
   
}
