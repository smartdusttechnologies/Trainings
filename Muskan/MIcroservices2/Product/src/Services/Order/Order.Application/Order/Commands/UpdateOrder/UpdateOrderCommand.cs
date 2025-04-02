using Ordering.Application.Order.Commands.CreateOrder;

namespace Ordering.Application.Order.Commands.UpdateOrder
{
    // Record type for the UpdateOrderCommand, which includes an OrdersDTO object
    public record UpdateOrderCommand(OrdersDTO Order) : ICommand<UpdateOrderResult>;

    // Record type for the result of the UpdateOrderCommand, which includes a boolean indicating success
    public record UpdateOrderResult(bool isSuccess);

    // Validator class for the UpdateOrderCommand
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            // Rule to ensure the Order ID is not empty
            RuleFor(x => x.Order.Id).NotEmpty().WithMessage("Id is required");

            // Rule to ensure the OrderName is not empty
            RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Name is required");

            // Rule to ensure the CustomerId is not null
            RuleFor(x => x.Order.CustomerId).NotNull().WithMessage("CustomerId is required");

            // Rule to ensure the OrderItems collection is not empty
            RuleFor(x => x.Order.OrderItems).NotEmpty().WithMessage("OrderItems should not be empty");
        }
    }
}
