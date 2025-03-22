namespace Ordering.Application.Order.Commands.CreateOrder
{
    
    /// <summary>
    /// Record type for the CreateOrderCommand, which includes an OrdersDTO object
    /// </summary>
    /// <param name="Order"></param>
    public record CreateOrderCommand(OrdersDTO Order)
        : ICommand<CreateOrderResult>; /// Implements ICommand with a result of type CreateOrderResult
    
    /// <ummary>
    /// Record type for the result of the CreateOrderCommand, which includes a Guid for the order ID
    /// </summary>
    /// <param name="Id"></param>
    public record CreateOrderResult(Guid Id);
    
    /// <summary>
    /// Validator class for the CreateOrderCommand
    /// </summary>
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            // Rule to ensure the OrderName is not empty
            RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Name is required");
            // Rule to ensure the CustomerId is not null
            RuleFor(x => x.Order.CustomerId).NotNull().WithMessage("CustomerId is required");
            // Rule to ensure the OrderItems collection is not empty
            RuleFor(x => x.Order.OrderItems).NotEmpty().WithMessage("OrderItems should not be empty");
        }
    }
}
