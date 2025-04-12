namespace Basket.API.Baskets.DeleteBasket
{
    public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;
    public record DeleteBasketResult(bool isSuccess);
    public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketCommandValidator()
        {
            RuleFor(x => x.UserName).NotNull().WithMessage("Username can not be null.");
        }
    }
    public class DeleteBasketHandler(IBasketRepository repository , ILoggingService<DeleteBasketHandler> logger)   : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
        {
     
                await logger.LogInformationAsync("Deleting basket for user: " + command.UserName);
           await repository.DeleteBasket(command.UserName, cancellationToken);
            return new DeleteBasketResult(true);
         
           
        }
    }
  
}
