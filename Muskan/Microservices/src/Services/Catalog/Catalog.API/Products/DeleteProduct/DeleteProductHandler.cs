

using Catalog.API.Exceptions;
using Catalog.API.Products.UpdateProduct;
using FluentValidation;

namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid id ) : ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool isSuccess);
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.id).NotEmpty().WithMessage("Id is required");
          }
    }
    internal class DeleteProductHandler(IProductRepository _productRepository) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var result = await _productRepository.DeleteProductAsync(command.id, cancellationToken);

            if (!result)
            {
                throw new ProductNotFoundException(command.id);
            }
            Console.WriteLine($"Product {command.id} deleted successfully.");
            return new DeleteProductResult(true);

        }


    }
}
