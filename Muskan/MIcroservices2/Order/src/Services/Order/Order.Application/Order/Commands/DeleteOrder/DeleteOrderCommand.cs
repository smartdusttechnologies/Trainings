using Ordering.Application.Order.Commands.UpdateOrder;

namespace Ordering.Application.Order.Commands.DeleteOrder
{
    /// <summary>
    /// Command to delete an order
    /// </summary>
    /// <param name="orderId">The ID of the order to delete</param>
    public record DeleteOrderCommand(Guid orderId) : ICommand<DeleteOrderResult>;

    /// <summary>
    /// Result of the delete order command
    /// </summary>
    /// <param name="isSuccess">Indicates if the deletion was successful</param>
    public record DeleteOrderResult(bool isSuccess);

    /// <summary>
    /// Validator for the DeleteOrderCommand
    /// </summary>
    public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(x => x.orderId).NotEmpty().WithMessage("Id is required");
        }
    }
}
